using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillManager : MonoBehaviour
{

    public static SkillManager instance; //

    [Header("Resources")]
    public GameObject rendererPrefab;

    [Header("Sprites")]
    public Material defaultMaterial;
    [Space]
    public Sprite electroSprite;
    public Material electroMaterial;
    [Space]
    public Sprite poisonFlaskSprite;
    public Sprite poisonSprite;
    public Material poisonMaterial;

    [Header("Settings")]
    public LayerMask enemyLayer;

    public Stack<SpriteRenderer> renderers = new Stack<SpriteRenderer>(10);
    public List<Skill> skills = new List<Skill>(10);

    void Awake()
    {
        instance = this;
        renderers = new Stack<SpriteRenderer>(10);
    }

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].Update()) // 스킬이 비활성화됨
            {
                i--;
            }
        }
    }

    public static SpriteRenderer Spawn()
    {
        if (instance.renderers.Count == 0)
        {
            GameObject newObj = Instantiate(instance.rendererPrefab, instance.transform);
            return newObj.GetComponent<SpriteRenderer>();
        }
        else
        {
            SpriteRenderer newRenderer = instance.renderers.Pop();
            if (!newRenderer) return Spawn();
            newRenderer.gameObject.SetActive(true);
            return newRenderer;
        }
    }

    public static void Return(SpriteRenderer renderer)
    {
        if (!renderer) return;
        renderer.gameObject.SetActive(false);
        instance.renderers.Push(renderer);
    }

    public static void Cast(Skill skill)
    {
        instance.skills.Add(skill);
    }

    public static void Dispose(Skill skill)
    {
        int index = instance.skills.IndexOf(skill);
        if (index > -1) instance.skills.RemoveAt(index);
    }

}

public abstract class Skill
{
    public float lifeTime;
    public SkillManager Manager => SkillManager.instance;

    public abstract bool Update();
    public abstract void Destroy();
}

/* Skills */

public class ElectroSkill : Skill
{
    public SpriteRenderer renderer;

    public ElectroSkill(Vector2 position, float radius, short damage)
    {
        lifeTime = 1f;
        renderer = SkillManager.Spawn();
        renderer.sprite = Manager.electroSprite;
        renderer.material = Manager.electroMaterial;
        renderer.transform.position = position + Vector2.up;
        renderer.transform.localScale = new Vector3(1f, 3f, 1f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, Manager.enemyLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                enemy.Damage(damage, position);
            }
        }
    }

    public override bool Update()
    {
        lifeTime -= Time.deltaTime;
        renderer.color = new Color(1f, 1f, 1f, lifeTime * 3f);
        renderer.transform.localScale = new Vector3(1f, 3f, 1f - lifeTime);
        if (lifeTime <= 0f)
        {
            Destroy();
            return true;
        }
        return false;
    }

    public override void Destroy()
    {
        SkillManager.Return(renderer);
        SkillManager.Dispose(this);
    }
}

public class PoisonFlaskSkill : Skill
{
    SpriteRenderer renderer;
    Vector2 position;
    Vector2 target;
    float radius;
    short damage;

    public PoisonFlaskSkill(Vector2 target, float radius, short damage)
    {
        lifeTime = 1f;
        renderer = SkillManager.Spawn();
        renderer.sprite = Manager.poisonFlaskSprite;
        renderer.material = Manager.defaultMaterial;
        renderer.color = Color.white;
        renderer.transform.position = Player.position;
        renderer.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        position = Player.position;
        this.target = target;
        this.radius = radius;
        this.damage = damage;
    }

    public override bool Update()
    {
        lifeTime -= Time.deltaTime * 2f;
        renderer.transform.position = Vector2.Lerp(target, position, lifeTime) + new Vector2(0f, Mathf.Sin(lifeTime * Mathf.PI));
        if (lifeTime <= 0f)
        {
            SkillManager.Cast(new PoisonAreaSkill(target, radius, damage));
            Destroy();
            return true;
        }
        return false;
    }

    public override void Destroy()
    {
        SkillManager.Return(renderer);
        SkillManager.Dispose(this);
    }
}

public class PoisonAreaSkill : Skill
{
    public SpriteRenderer renderer;

    float coolTime = 0f;
    float radius;
    short damage;
    Vector2 position;

    public PoisonAreaSkill(Vector2 position, float radius, short damage)
    {
        lifeTime = 1f;
        renderer = SkillManager.Spawn();
        renderer.sprite = Manager.poisonSprite;
        renderer.material = Manager.poisonMaterial;
        renderer.transform.position = position;
        renderer.transform.localScale = new Vector3(4f, 4f, 0f);

        this.position = position;
        this.radius = radius;
        this.damage = damage;
    }

    public override bool Update()
    {
        lifeTime -= Time.deltaTime;
        coolTime -= Time.deltaTime;
        renderer.color = new Color(1f, 1f, 1f, lifeTime * 10f);
        renderer.transform.localScale = new Vector3(4f, 4f, 1f - lifeTime);
        if (coolTime <= 0f)
        {
            coolTime = 0.25f;
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, Manager.enemyLayer);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out EnemyBase enemy))
                {
                    enemy.Damage(damage, position);
                }
            }
        }
        if (lifeTime <= 0f)
        {
            Destroy();
            return true;
        }
        return false;
    }

    public override void Destroy()
    {
        SkillManager.Return(renderer);
        SkillManager.Dispose(this);
    }
}