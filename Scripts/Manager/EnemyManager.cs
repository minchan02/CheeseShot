using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using NaughtyAttributes;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    [Header("'나올 확률이 높은 적'은 배열 안에 더 많이 배치하여 확률을 높일 수 있습니다."), Expandable]
    public Enemy[] enemies;

    [Header("Settings"), Tooltip("적이 소환될 때 플레이어와 얼마나 떨어진 곳에서 소환될지 결정합니다")]
    public float maxSpawnDelay;
    public float minSpawnDelay;
    public float spawnRadius;
    public int spawnAmount;
    public float maxEnemyDist;
    public int maxEnemyCount;
    public float spawnAmountOverLifetime; // 시간 당 소환되는 적 수 (초 단위)
    public int startSpawnAmount; // 초기에 소환되는 적 수
    public int maxSpawnAmount; // 최대로 소환되는 적 수

    [Header("Resources")]
    public Mesh mesh;

    private Dictionary<Type, EnemyPool> pools;
    private Type[] enemyTypes;

    public static int totalCount;

    private float totalTime;
    private float leftSpawnTime;

    void Awake()
    {
        instance = this;
        totalCount = 0;
    }

    void Start()
    {
        pools = new Dictionary<Type, EnemyPool>();
        enemyTypes = new Type[enemies.Length];
        List<Type> types = new List<Type>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy profile = enemies[i];
            Type enemyType = profile.prefab.GetComponent<EnemyBase>().GetType();
            enemyTypes[i] = enemyType;
            if (!types.Contains(enemyType))
            {
                GameObject enemyPrefab = profile.prefab;
                pools.Add(enemyType, new EnemyPool(() =>
                {
                    GameObject enemyObject = Instantiate(enemyPrefab, transform);
                    return enemyObject.GetComponent<EnemyBase>();
                },
                enemy =>
                {
                    Destroy(enemy.gameObject);
                },
                mesh, profile));
                types.Add(enemyType);
            }
        }
    }

    void Update()
    {
        if (totalCount >= maxEnemyCount) return;
        totalTime += Time.deltaTime;
        leftSpawnTime -= Time.deltaTime;
        if (leftSpawnTime <= 0)
        {
            leftSpawnTime = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
            int count = Mathf.Clamp(startSpawnAmount + spawnAmount * Player.instance.TotalCount + Mathf.FloorToInt(totalTime * spawnAmountOverLifetime), 0, maxSpawnAmount);
            for (int i = 0; i < count; i++) RandomSpawn();
        }
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<Type, EnemyPool> pool in pools)
        {
            pool.Value.Render();
        }
    }

    public void RandomSpawn()
    {
        Type info = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
        Vector2 position = (Vector2)Player.instance.transform.position + UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        Spawn(info, position);
    }

    public EnemyBase Spawn(Type type, Vector2 position)
    {
        return pools[type].Get(position);
    }

    public void Kill(EnemyBase enemy)
    {
        pools[enemy.GetType()].Release(enemy);
    }

    public void Kill(Type type, EnemyBase enemy)
    {
        pools[type].Release(enemy);
    }

    public void KillAll()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.activeInHierarchy && child.TryGetComponent(out EnemyBase enemy))
            {
                pools[enemy.GetType()].Release(enemy);
            }
        }
    }

}

[Serializable]
public class EnemyPool
{

    private readonly List<EnemyBase> inactive;
    public readonly List<EnemyBase> active;
    private readonly Func<EnemyBase> onCreate;
    private readonly Action<EnemyBase> onDestroy;

    private readonly List<Matrix4x4> matrices;

    private readonly int maxCount;

    private readonly Mesh mesh;
    private readonly Enemy profile;

    public EnemyPool(Func<EnemyBase> onCreate, Action<EnemyBase> onDestroy, Mesh mesh, Enemy profile, int maxCount = 1000, int startCount = 10)
    {
        this.onCreate = onCreate;
        this.onDestroy = onDestroy;
        this.maxCount = maxCount;
        inactive = new List<EnemyBase>(startCount);
        active = new List<EnemyBase>(startCount);
        matrices = new List<Matrix4x4>(startCount);

        this.mesh = mesh;
        this.profile = profile;
        
        for (int i = 0; i < startCount; i++)
        {
            EnemyBase element = onCreate();
            element.Final();
            inactive.Add(element);
        }
    }

    public EnemyBase Get(Vector2 position)
    {
        EnemyBase result;

        if (inactive.Count == 0)
        {
            result = onCreate();
        }
        else
        {
            result = inactive[inactive.Count - 1];
            inactive.RemoveAt(inactive.Count - 1);
        }

        active.Add(result);
        matrices.Add(Matrix4x4.zero);
        EnemyManager.totalCount++;

        result.Init(position);
        return result;
    }

    public void Release(EnemyBase element)
    {
        if (active.Count == 0 || inactive.Contains(element) || !active.Contains(element)) Debug.LogError("asdf");

        element.Final();
        active.Remove(element);
        matrices.RemoveAt(matrices.Count - 1);

        if (inactive.Count < maxCount) inactive.Add(element);
        else onDestroy.Invoke(element);

        EnemyManager.totalCount--;
    }

    public void Render()
    {
        if (active == null || active.Count == 0) return;

        int count = active.Count;
        for (int i = 0; i < count; i++)
        {
            Transform activeTransform = active[i].transform;
            matrices[i] = Matrix4x4.TRS(activeTransform.position, activeTransform.rotation, activeTransform.lossyScale);
        }

        Graphics.DrawMeshInstanced(mesh, 0, profile.material, matrices);
    }

}