using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [BoxGroup("�� ��ȯ")] public float maxSpawnDelay;
    [BoxGroup("�� ��ȯ")] public float minSpawnDelay;
    [BoxGroup("�� ��ȯ")] public float spawnRadius;
    [BoxGroup("�� ��ȯ")] public int spawnAmount;
    [BoxGroup("�� ��ȯ")] public float maxEnemyDist;
    [BoxGroup("�� ��ȯ")] public int maxEnemyCount;
    [BoxGroup("�� ��ȯ")] public float spawnAmountOverLifetime; // �ð� �� ��ȯ�Ǵ� �� �� (�� ����)
    [BoxGroup("�� ��ȯ")] public int startSpawnAmount; // �ʱ⿡ ��ȯ�Ǵ� �� ��
    [BoxGroup("�� ��ȯ")] public int maxSpawnAmount; // �ִ�� ��ȯ�Ǵ� �� ��

    [Expandable] public Enemy[] enemyList;

 //   [Foldout("�÷��̾�")] public GameObject shopPrefab;

    [Foldout("��")] public Mesh enemyMesh;
    [Foldout("��")] public Material enemyMaterial;
    [Foldout("��")] public float enemyNormalDropPrecent;
    [Foldout("��")] public GameObject[] enemyNormalDroppable;

    //[Foldout("UI")] public GameObject menuSet;
    [Foldout("UI")] public GameObject skillPanel;
    [Foldout("UI")] public GameObject MenuPanel;
    [Foldout("UI")] public GameObject stopBtn;
    [Foldout("UI")] public GameObject gameOverSet;
    [Foldout("UI")] public TextMeshProUGUI scoreText; //Text���� ��ü => TextMeshPro��? Text�� ��� ����
    [Foldout("UI")] public RectTransform healthBar;
    [Foldout("UI")] public CanvasGroup healthBarGroup;
    [Foldout("UI")] public float healthBarScaleSpeed;
    [Foldout("UI")] public float healthBarFadeSpeed;

    [Foldout("ȿ��")] public ParticleSystem hitEffect;
    [Foldout("ȿ��")] public ParticleSystem bulletRemoveEffect;
    [Foldout("ȿ��")] public ParticleSystem fieldPoisonEffect;
    [Foldout("ȿ��")] public ParticleSystem lightningEffect;
    [Foldout("ȿ��")] public ParticleSystem fieldLightningEffect;

    [Foldout("�����")] public GameObject soundManagerPrefab;
    [Foldout("�����")] public AudioClip buttonAudioClip;
    [Foldout("�����")] public AudioClip AttackAudioClip;

    [Foldout("�����")] [ReadOnly] public bool isPause;
    [Foldout("�����")] [ReadOnly] public int score;
    [Foldout("�����")] [ReadOnly] public int kills;

    private float totalTime;
    private int cashPerKill;

    void Awake()
    {
        instance = this;
    }

    async void Start()
    {
        if (!SoundManager.instance) Instantiate(soundManagerPrefab, Vector3.zero, Quaternion.identity);
        Pause(false);
        UpdateLife();
        cashPerKill = UpgradeManager.Get(UpgradeManager.Preset.cashPerKill);
    }
    
    void Update()
    {
        totalTime += Time.deltaTime;

        //�Ͻ�����
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(!isPause);
        }
        healthBar.sizeDelta = Vector2.MoveTowards(healthBar.sizeDelta, new Vector2(Player.instance.health / (float)Player.instance.maxHealth * 610f, 42), Time.deltaTime * healthBarScaleSpeed);
        healthBarGroup.alpha = Mathf.MoveTowards(healthBarGroup.alpha, 0.25f, Time.deltaTime * healthBarFadeSpeed);
    }

    private void OnGUI() // ����� â GUI
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;
        GUILayout.BeginArea(new Rect(50, 50, 500, 200), GUI.skin.box);
        GUILayout.Label($"óġ�� �� �� : {kills}", style);
        GUILayout.Label($"��ȯ�Ǵ� �� �� : {Mathf.Clamp(startSpawnAmount + spawnAmount * Player.instance.TotalCount + Mathf.FloorToInt(totalTime * spawnAmountOverLifetime), 0, maxSpawnAmount)}", style);
        GUILayout.Label($"����ִ� �� �� : {EnemyManager.totalCount}", style);
        GUILayout.EndArea();
    }

    void OnDestroy()
    {
        Pause(false);
    }

    //--------------------�׼�--------------------//

    public void UpdateLife()
    {
        healthBarGroup.alpha = 1;
    }

    public void UpdateScore()
    {
        scoreText.text = string.Format("{0}", score);
    }

    public void HitEffect(Vector2 position)
    {
        hitEffect.transform.position = position;
        hitEffect.Emit(1);
    }

    public void BulletRemoveEffect(Vector2 position)
    {
        bulletRemoveEffect.transform.position = position;
        bulletRemoveEffect.Emit(1);
    }

    public void FieldPoisonEffect(Vector2 position, float radius)
    {
        ParticleSystem.EmitParams EP = new ParticleSystem.EmitParams { position = position, startSize = radius };
        fieldPoisonEffect.Emit(EP, 1);
    }

    public void LightningEffect(Vector2 position, float radius)
    {
        lightningEffect.transform.position = position + new Vector2(0, 2);
        lightningEffect.Emit(1);
        StartCoroutine(FieldLightningEffect(position, radius));
    }

    IEnumerator FieldLightningEffect(Vector2 position, float radius)
    {
        yield return new WaitForSeconds(0.1f);
        ParticleSystem.EmitParams EP = new ParticleSystem.EmitParams { position = position, startSize = radius };
        fieldLightningEffect.Emit(EP, 1);
    }

    public void DropNormal(Vector2 position)
    {
        if (GetRandomPrecent(enemyNormalDropPrecent / (1 + Player.instance.TotalCount)))
        {
            GameObject drop = enemyNormalDroppable[GetRandomInt(0, enemyNormalDroppable.Length - 1)];
            if (drop != null)
            {
                Instantiate(drop, position, Quaternion.identity);
            }
        }
    }

    public void AddKill(Enemy enemy)
    {
        kills++;
        DBManager.DB.cash += enemy.price * cashPerKill;
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        Player.instance.transform.position = Vector3.zero;
        Player.instance.isHit = false;
        MapManager.instance.UpdateTiles();
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        Pause(false);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void InversePause() //�Ͻ����� ������ ��� �Ͻ����� ���� / �ƴ� ��� �Ͻ����� ���� ����
    {
        if(MenuPanel.activeInHierarchy == true)
        {
            Pause(false);
        }

        else
        {
            Pause(true);
        }
        // Pause(!isPause);
        MenuPanel.SetActive(isPause);
    }

    public void InverseSkillUI()
    {
        Pause(!isPause);
        skillPanel.SetActive(isPause);
        stopBtn.SetActive(!isPause);
    }
    public void Pause(bool pause)
    {
        // skillPanel.SetActive(pause);
        Time.timeScale = pause ? 0 : 1; //pause == true��� 0 ��ȯ, pause == false��� 1 ��ȯ
        isPause = pause;
    }

    public void ButtonSound()
    {
        SoundManager.instance.SFXPlay(buttonAudioClip);
    }

    public void AttackSound()
    {
        SoundManager.instance.SFXPlay(AttackAudioClip);
    }

    //--------------------�Լ�--------------------//

    public static int GetRandomInt(int min, int max)
    {
        float random = UnityEngine.Random.Range((float)min - 0.49f, (float)max + 0.49f);
        return Mathf.RoundToInt(random);
    }

    public static bool GetRandomPrecent(float percent)
    {
        return UnityEngine.Random.Range(0f, 1f) <= percent;
    }


}
