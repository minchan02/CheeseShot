using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    public static Player instance;
    public static Vector2 position;

    [Header("외부 클래스")]
    public GameManager gameManager;
    public ItemManager ItemManager;
    public Item Cuitem; // 현재 아이템
    public Joystick joystick; //조이스틱

    [Header("플레이어 정보")]
    public int health;
    [Space]
    public int maxHealth;
    public int speed;
    public int power;
    public int bulletSpeed;
    public int bulletSize;
    public float bulletRate;
    public float bulletRange;

    [Header("공격 노드")]
    public int Up;
    public int Down;
    public int Right;
    public int Left;
    public int RightUp;
    public int LeftUp;
    [Space]
    public int TotalCount;

    public enum Element { None, Fire, Water, Nature, Electro, Wind, Poison, Ground };

    [Header("속성 노드")]
    public Element elementA;
    public Element elementB;
    public Element elementC;

    [Header("노드")]
    public int SlotIdx = 0;    // 노드 자리
    public List<Slot> slotList = new List<Slot>(); // 슬롯 리스트

    [Header("체력")]
    public float hitDelay; //맞은 후 hitDelay초 후에 유령상태 해제
    public float nockback;

    [Header("발사")]
    public Projectile bullet;
    public Vector2 shootOffset;
    public float shootAngle; //두발 쏘기 / 세발 쏘기 총알 사이 간격
    public float shootDist;
    public LayerMask enemyLayer;

    [Header("UI")]
    public GameObject NodeChange;  // 노드 스킬 슬롯 꽉찼을 때 변경
    public GameObject NodeChangeBtn;  // 노드 스킬 슬롯 꽉찼을 때 변경 유무 뜨는 창입니다
    public GameObject SelectGem;
    public GameObject CautionChange;
    public Image ChangeGem; // 바꿀 젬
    public Image BeforeGem; // 바꾸기 이전 젬
    public GameObject Question;
    public GameObject OffOkBtn;
    public GameObject ChangeOkBtn;

    [Header("애니메이션")]
    public float AttackDuration;

    [HideInInspector] public float curShotDelay; //발사 로직 현재
    [HideInInspector] public bool isHit; //피격 / [HideInInspector]를 붙여서 다른 스크립트에선 접근 가능하지만 에디터에서 더러워 보이므로 에디터에서 보이지 않도록 설정
    
    private Rigidbody2D rb;
    private Renderer thisRenderer;
    private Animator anim;
    private int curType;
    private int Slotnum = -1;

    private SkillHolder skill1;
    private SkillHolder skill2;
    private SkillHolder skill3;

    //--------------------업데이트--------------------//

    void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        thisRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        // Test
        skill1 = new PoisonFlaskSkillHolder();
        skill2 = new Electro1SkillHolder();

        // Setup
        maxHealth = UpgradeManager.Get(UpgradeManager.Preset.healthPerUpgrade);
        power = UpgradeManager.Get(UpgradeManager.Preset.powerPerUpgrade);
        health = maxHealth;
        TotalCount = Up + Down + Right + Left + RightUp + LeftUp;
        position = transform.position;
    }

    void Update()
    {
        Reload();
        Fire();
        Move();

        position = transform.position;

        Skill();
    }

    //--------------------액션--------------------//

    public void Move() //조이스틱
    {
        if (!isHit) rb.velocity = joystick.GetControl() * speed;

        anim.SetFloat("inputx", rb.velocity.x);
        anim.SetFloat("inputy", rb.velocity.y);
    }

    public void Reload()
    {
        curShotDelay += Time.deltaTime; //현재 시간 추가
    }

    public void Fire()
    {
        if (curShotDelay < 1f / bulletRate) return;

        Collider2D enemy = Scan(transform.position);
        if (!enemy) return;

        Vector2 shootPosition = (Vector2)transform.position + shootOffset;
        Vector2 dirVec = (Vector2)enemy.transform.position - shootPosition;
        FireBullet(dirVec.normalized);
        gameManager.AttackSound(); // 타격 사운드
        curShotDelay = 0; //총알을 쏘고 난 후에는 변수 0으로 초기화
        anim.SetBool("attacking", true);
        CancelInvoke("ResetAnim");
        Invoke("ResetAnim", AttackDuration);
    }

    public static Collider2D Scan(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, instance.bulletRange, instance.enemyLayer);

        float closest = Mathf.Infinity;
        Collider2D target = null;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                float dist = Vector2.SqrMagnitude(position - (Vector2)hits[i].transform.position);
                if (dist < closest)
                {
                    closest = dist; // 가장 가까운 얘 찾기
                    target = hits[i];
                }
            }
        }
        return target;
    }

    public static Collider2D[] ScanAll(Vector2 position)
    {
        return Physics2D.OverlapCircleAll(position, instance.bulletRange, instance.enemyLayer);
    }

    public void FireBullet(Vector2 direction)
    {
        Vector3 shootPosition = transform.position + (Vector3)shootOffset;
        for(int i = 0; i < Up; i++)
        {
            Shoot((i - Up / 2f) * shootAngle, direction, shootPosition);
        }
        for (int i = 0; i < Down; i++)
        {
            Shoot((i - Down / 2f) * shootAngle + 180, direction, shootPosition);
        }
        for (int i = 0; i < Right; i++)
        {
            Shoot((i - Right / 2f) * shootAngle - 90, direction, shootPosition);
        }
        for (int i = 0; i < Left; i++)
        {
            Shoot((i - Left / 2f) * shootAngle + 90, direction, shootPosition);
        }
        for (int i = 0; i < RightUp; i++)
        {
            Shoot((i - RightUp / 2f) * shootAngle - 45, direction, shootPosition);
        }
        for (int i = 0; i < LeftUp; i++)
        {
            Shoot((i - LeftUp / 2f) * shootAngle + 45, direction, shootPosition);
        }
    }

    public void Shoot(float angle, Vector2 direction, Vector2 position)
    {
        Vector2 dirOff = Quaternion.Euler(0, 0, angle) * direction.normalized;
        /* // 임시 비활성화
        int sprite = 0;
        int type = 0;
        bool water = elementA == Element.Water || elementB == Element.Water || elementC == Element.Water;
        bool fire = elementA == Element.Fire || elementB == Element.Fire || elementC == Element.Fire;
        if(water && fire)
        {
            bool left = curType % 2 == 0;
            sprite = left ? 1: 2;
            type = left ? 1 : 2;
            curType++;
            if(curType >= 100)
            {
                curType -= 100;
            }
        }
        else if (water)
        {
            sprite = 1;
            type = 1;
        }
        else if (fire)
        {
            sprite = 2;
            type = 2;
        }
        */
        ProjectileManager.instance.Spawn(bullet, position + dirOff * shootDist, dirOff * (3f + bulletSpeed));
    }

    public void Skill()
    {
        skill1?.Update();
        skill2?.Update();
        skill3?.Update();
    }

    public void ResetAnim()
    {
        anim.SetBool("attacking", false);
    }

    public bool Damage(int damage, Vector2 direction)
    {
        if (isHit) return false;
        health -= damage;
        if(health <= 0)
        {
            gameManager.GameOver();
        }
        isHit = true;
        gameManager.UpdateLife();
        Invoke("UnHit", hitDelay);
        thisRenderer.material.SetFloat("_Hit", 1);
        rb.velocity = direction * nockback;
        return true;
    }

    public void UnHit()
    {
        isHit = false;
        thisRenderer.material.SetFloat("_Hit", 0);
    }

    public void AddAttackNode()
    {
        if (TotalCount >= 50) return;
        int index = GameManager.GetRandomInt(0, 5);
        switch (index)
        {
            case 0: Up++; break;
            case 1: Down++; break;
            case 2: Right++; break;
            case 3: Left++; break;
            case 4: RightUp++; break;
            case 5: LeftUp++; break;
        }
        TotalCount = Up + Down + Right + Left + RightUp + LeftUp;
    }

    public void AddPropertyNode(Element element)
    {
        if(elementA == Element.None)
        {
            elementA = element;
            // 스킬 지정
        }
        else if (elementB == Element.None)
        {
            elementB = element;
            // 스킬 지정
        }
        else if (elementC == Element.None)
        {
            elementC = element;
            // 스킬 지정
        }
        else
        {
            //속성 노드 교체
        }
    }

    //--------------------노드 스킬 시스템--------------------//
    public void ChangeNodeOn()
    {
        NodeChange.SetActive(true);
        NodeChangeBtn.SetActive(false);

        if(Slotnum == -1)
        {
            OffOkBtn.SetActive(true);
            ChangeOkBtn.SetActive(false);
        }
    }

    public void ChangeNodeOff()
    {
        NodeChangeBtn.SetActive(false);
        ItemManager.itemidx[Elementidx(Cuitem.itemName)] = 0;
    }

    public void CautionWaste()
    {
        SelectGem.SetActive(false);
        CautionChange.SetActive(true);
    }
    public void WasteGem()
    {
        CautionChange.SetActive(false);
    }

    public void CancelWaste()
    {
        CautionChange.SetActive(false);
        SelectGem.SetActive(true);
    }

    public void SetNode(Item item)
    {
        Cuitem = item;
        if(SlotIdx <= 2)    // 슬롯이 꽉 차지 않았을 때
        {
            slotList[SlotIdx].gameObject.SetActive(true);
            slotList[SlotIdx].itemIcon.sprite = item.itemImage;
            slotList[SlotIdx].item = item;
            switch (SlotIdx)
            {
                case 0:
                    elementA = SelectElement(item.itemName);
                    break;
                case 1:
                    elementB = SelectElement(item.itemName);
                    break;
                case 2:
                    elementC = SelectElement(item.itemName);
                    break;
            }
            SlotIdx++;
        }
        else    // 슬롯이 꽉 찼을 때
        {
            NodeChangeBtn.SetActive(true);
            ChangeGem.sprite = item.itemImage;
        }
        
    }

    public Element SelectElement(string name) =>   // 이름 = 속성
        name switch
        {
            "Fire"      => Element.Fire,
            "Water"     => Element.Water,
            "Nature"    => Element.Nature,
            "Electro"   => Element.Electro,
            "Poison"    => Element.Poison,
            "Ground"    => Element.Ground,
            "Wind"      => Element.Wind,
            _           => Element.None
        };

    public int Elementidx(string name)   // 속성 번호
    {
        return name switch
        {
            "Fire" => 0,
            "Water" => 1,
            "Nature" => 2,
            "Electro" => 4,
            "Poison" => 5,
            "Ground" => 6,
            "Wind" => 3,
            _ => -1
        };
    }

    public void SelectAttributeSlot(int Slotnum) // 젬이 꽉 찼을때
    {
        Question.SetActive(false);
        BeforeGem.sprite = slotList[Slotnum].itemIcon.sprite;
        BeforeGem.gameObject.SetActive(true);
        this.Slotnum = Slotnum;
        OffOkBtn.SetActive(false);
        ChangeOkBtn.SetActive(true);
    }

    public void ChangeSlot()
    {
        ItemManager.itemidx[Elementidx(slotList[Slotnum].item.itemName)] = 0;
        slotList[Slotnum].item = Cuitem;
        slotList[Slotnum].itemIcon.sprite = Cuitem.itemImage;
        switch (Slotnum)
        {
            case 0:
                elementA = SelectElement(Cuitem.itemName);
                break;
            case 1:
                elementB = SelectElement(Cuitem.itemName);
                break;
            case 2:
                elementC = SelectElement(Cuitem.itemName);
                break;
        }

        Slotnum = -1;
        BeforeGem.gameObject.SetActive(false);
        Question.SetActive(true);
        NodeChange.SetActive(false);
    }

    public void CancelChange()
    {
        BeforeGem.gameObject.SetActive(false);
        Question.SetActive(true);
        NodeChange.SetActive(false);
        Slotnum = -1;

        SelectGem.SetActive(true);
    }

    //--------------------함수--------------------//

    public static float SqrDistance(Vector3 a, Vector3 b)
    {
        Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
    }

    public static short GetDamage(float persent)
    {
        return (short)(instance.power * persent * 0.01f);
    }

    //--------------------물리--------------------//

    /*void OnTriggerEnter2D(Collider2D collision) //적과 충돌
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if (isHit) return;
            Damage(1, (transform.position - collision.transform.position).normalized);
            //Destroy(collision.gameObject);
        }
    }*/

}

public abstract class SkillHolder
{
    public abstract void Update();
}

/* Skill Holders */

public class Electro1SkillHolder : SkillHolder
{
    public float coolTime = 1f;

    public override void Update()
    {
        coolTime -= Time.deltaTime;
        if (coolTime <= 0f)
        {
            coolTime = 1f;
            SkillManager.Cast(new ElectroSkill(Player.position + Random.insideUnitCircle * 5f, 1f, 10));
        }
    }
}

public class PoisonFlaskSkillHolder : SkillHolder
{
    public float coolTime = 1f;

    public override void Update()
    {
        coolTime -= Time.deltaTime;
        if (coolTime <= 0f)
        {
            coolTime = 1f;
            SkillManager.Cast(new PoisonFlaskSkill(Player.position + Random.insideUnitCircle * 5f, 2f, Player.GetDamage(100)));
        }
    }
}
