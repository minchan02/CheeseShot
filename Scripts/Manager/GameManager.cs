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

    [BoxGroup("적 소환")] public float maxSpawnDelay;
    [BoxGroup("적 소환")] public float minSpawnDelay;
    [BoxGroup("적 소환")] public float spawnRadius;
    [BoxGroup("적 소환")] public int spawnAmount;
    [BoxGroup("적 소환")] public float maxEnemyDist;
    [BoxGroup("적 소환")] public int maxEnemyCount;
    [BoxGroup("적 소환")] public float spawnAmountOverLifetime; // 시간 당 소환되는 적 수 (초 단위)
    [BoxGroup("적 소환")] public int startSpawnAmount; // 초기에 소환되는 적 수
    [BoxGroup("적 소환")] public int maxSpawnAmount; // 최대로 소환되는 적 수

    [Expandable] public Enemy[] enemyList;

 //   [Foldout("플레이어")] public GameObject shopPrefab;

    [Foldout("적")] public Mesh enemyMesh;
    [Foldout("적")] public Material enemyMaterial;
    [Foldout("적")] public float enemyNormalDropPrecent;
    [Foldout("적")] public GameObject[] enemyNormalDroppable;

    //[Foldout("UI")] public GameObject menuSet;
    [Foldout("UI")] public GameObject skillPanel;
    [Foldout("UI")] public GameObject MenuPanel;
    [Foldout("UI")] public GameObject stopBtn;
    [Foldout("UI")] public GameObject gameOverSet;
    [Foldout("UI")] public TextMeshProUGUI scoreText; //Text에서 교체 => TextMeshPro란? Text의 고급 버전
    [Foldout("UI")] public RectTransform healthBar;
    [Foldout("UI")] public CanvasGroup healthBarGroup;
    [Foldout("UI")] public float healthBarScaleSpeed;
    [Foldout("UI")] public float healthBarFadeSpeed;

    [Foldout("효과")] public ParticleSystem hitEffect;
    [Foldout("효과")] public ParticleSystem bulletRemoveEffect;
    [Foldout("효과")] public ParticleSystem fieldPoisonEffect;
    [Foldout("효과")] public ParticleSystem lightningEffect;
    [Foldout("효과")] public ParticleSystem fieldLightningEffect;

    [Foldout("오디오")] public GameObject soundManagerPrefab;
    [Foldout("오디오")] public AudioClip buttonAudioClip;
    [Foldout("오디오")] public AudioClip AttackAudioClip;

    [Foldout("디버그")] [ReadOnly] public bool isPause;
    [Foldout("디버그")] [ReadOnly] public int score;
    [Foldout("디버그")] [ReadOnly] public int kills;

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

        //일시정지
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(!isPause);
        }
        healthBar.sizeDelta = Vector2.MoveTowards(healthBar.sizeDelta, new Vector2(Player.instance.health / (float)Player.instance.maxHealth * 610f, 42), Time.deltaTime * healthBarScaleSpeed);
        healthBarGroup.alpha = Mathf.MoveTowards(healthBarGroup.alpha, 0.25f, Time.deltaTime * healthBarFadeSpeed);
    }

    private void OnGUI() // 디버그 창 GUI
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;
        GUILayout.BeginArea(new Rect(50, 50, 500, 200), GUI.skin.box);
        GUILayout.Label($"처치한 적 수 : {kills}", style);
        GUILayout.Label($"소환되는 적 수 : {Mathf.Clamp(startSpawnAmount + spawnAmount * Player.instance.TotalCount + Mathf.FloorToInt(totalTime * spawnAmountOverLifetime), 0, maxSpawnAmount)}", style);
        GUILayout.Label($"살아있는 적 수 : {EnemyManager.totalCount}", style);
        GUILayout.EndArea();
    }

    void OnDestroy()
    {
        Pause(false);
    }

    //--------------------액션--------------------//

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

    public void InversePause() //일시정지 상태인 경우 일시정지 해제 / 아닌 경우 일시정지 상태 설정
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
        Time.timeScale = pause ? 0 : 1; //pause == true라면 0 반환, pause == false라면 1 반환
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

    //--------------------함수--------------------//

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
