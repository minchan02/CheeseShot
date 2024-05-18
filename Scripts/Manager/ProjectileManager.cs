using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ProjectileManager : MonoBehaviour
{

    public static ProjectileManager instance;

    [BoxGroup("외부 클래스")] public GameManager gameManager;

    [Header("Settings")]
    public Projectile[] projectiles;
    public LayerMask enemyLayer;

    [Header("Resources")]
    public Mesh mesh;

    private Dictionary<Projectile, ProjectilePool> pools;

    //--------------------업데이트--------------------//

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pools = new Dictionary<Projectile, ProjectilePool>();
        for (int i = 0; i < projectiles.Length; i++)
        {
            Projectile profile = projectiles[i];
            pools.Add(profile, new ProjectilePool(mesh, profile, enemyLayer));
        }
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<Projectile, ProjectilePool> pool in pools)
        {
            pool.Value.Render();
        }
    }

    //--------------------액션--------------------//

    public void Spawn(Projectile projectile, Vector2 position, Vector2 direction)
    {
        if (!pools.ContainsKey(projectile)) pools.Add(projectile, new ProjectilePool(mesh, projectile, enemyLayer));
        pools[projectile].Spawn(position, direction);
    }

    public void KillAll()
    {
        foreach (KeyValuePair<Projectile, ProjectilePool> pool in pools)
        {
            pool.Value.projectiles.Clear();
        }
    }

}

[Serializable]
public class ProjectilePool
{

    public readonly List<ProjectileInfo> projectiles;

    private readonly List<Matrix4x4> matrices;

    private readonly int maxCount;

    private readonly Mesh mesh;
    private readonly Projectile profile;
    private readonly Material material;
    private readonly LayerMask enemyLayer;
    private readonly Vector3 scale;
    private readonly float speed;
    private readonly float angle;

    private float refreshTime = 0f;

    public ProjectilePool(Mesh mesh, Projectile profile, LayerMask enemyLayer, int maxCount = 1000, int startCount = 10)
    {
        this.maxCount = maxCount;
        projectiles = new List<ProjectileInfo>(startCount);
        matrices = new List<Matrix4x4>(startCount);

        this.mesh = mesh;
        this.profile = profile;
        this.enemyLayer = enemyLayer;
        material = profile.material;
        scale = profile.scale;
        speed = profile.speed;
        angle = profile.angle;
        refreshTime = UnityEngine.Random.value;
    }

    public void Spawn(Vector2 position, Vector2 direction)
    {
        if (projectiles.Count >= maxCount) projectiles.RemoveAt(0);
        projectiles.Add(new ProjectileInfo(position, direction * speed, angle));
        matrices.Add(Matrix4x4.identity);
    }

    public void Render()
    {
        if (projectiles == null || projectiles.Count == 0) return;

        int count = projectiles.Count;
        float deltaTime = Time.deltaTime;
        refreshTime += deltaTime;
        if (refreshTime > 1f)
        {
            refreshTime = 0f;
            for (int i = 0; i < count; i++)
            {
                ProjectileInfo info = projectiles[i];
                info.lifeTime--;
                if (info.lifeTime < 1)
                {
                    projectiles.RemoveAt(i);
                    matrices.RemoveAt(matrices.Count - 1);

                    i--;
                    count--;
                    continue;
                }
                projectiles[i] = info;
            }
        }
        for (int i = 0; i < count; i++)
        {
            ProjectileInfo info = projectiles[i];
            Vector2 translation = info.position + info.direction * deltaTime;
            RaycastHit2D hit = Physics2D.Linecast(info.position, translation, enemyLayer);
            if (hit && hit.collider.gameObject.activeInHierarchy && hit.collider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.Damage(profile.damage, info.position);

                projectiles.RemoveAt(i);
                matrices.RemoveAt(matrices.Count - 1);

                i--;
                count--;
                continue;
            }
            info.position = translation;
            projectiles[i] = info;
            matrices[i] = Matrix4x4.TRS(translation, info.rotation, scale);
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }

    public struct ProjectileInfo
    {
        public Vector2 position;
        public Vector2 direction;
        public Quaternion rotation;
        public short lifeTime;

        public ProjectileInfo(Vector2 position, Vector2 direction, float angle = 0f)
        {
            this.position = position;
            this.direction = direction;
            this.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angle);
            lifeTime = 4;
        }

        public ProjectileInfo(Vector2 position, Vector2 direction, Quaternion rotation)
        {
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            lifeTime = 4;
        }
    }

}