using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //Ÿ�ϸ� �ý����� �̿��ϱ� ����
using NaughtyAttributes;

public class MapManager : MonoBehaviour
{

    public static MapManager instance;

    [Foldout("�ܺ� Ŭ����")] public Tilemap tilemap; //���Ǹ� ���� ���� ���̹� ����

    [BoxGroup("����")] public int seed; //�� ���ϰ� ���� ����
    [BoxGroup("����")] public Vector2Int amount = new Vector2Int(1, 2); //ȭ�鿡 ������ Ÿ���� ���� / ���� Size.x*2, ���� Size.y*2 ��ŭ ȭ�鿡 ���̰� ��
    [BoxGroup("����")] public float tilemapScale; //Grid ������Ʈ�� ũ��
    [BoxGroup("����")] public float updateRate; //Ÿ���� ���ʸ��� ������Ʈ �� ���ΰ�?
    [BoxGroup("����")] public List<TileBase> tiles; //ǥ���� Ÿ�ϸ� ����Ʈ. TileBase[] �� ����

    [Foldout("�����")] [ReadOnly] public Vector2Int lastPos;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        seed = Random.Range(-10000, 10000); //���� �õ� ���� / �ε�� ����� �õ� ���
        InvokeRepeating("UpdateTiles", 0, updateRate); //���� ����� ���� UpdateTiles()�� updateRate�� ���� ����
    }

    void Update()
    {
        
    }

    public void UpdateTiles()
    {
        Vector2Int playerPos = Vector2Int.RoundToInt(Player.instance.transform.position / tilemapScale);
        if (playerPos != lastPos)
        {
            tilemap.ClearAllTiles(); //��� Ÿ�� ����, ������ �ʴ� Ÿ�� ���� ����
            for (int x = -amount.x; x < amount.x; x++)
            {
                for (int y = -amount.y; y < amount.y; y++)
                {
                    Vector2Int pos = playerPos + new Vector2Int(x, y); //ĳ�� / ���� ����� ������ �ʿ��� ���
                    int height = GetNoise(pos);
                    tilemap.SetTile((Vector3Int)pos, tiles[height]); //pos�� Ÿ�� ��ġ / Seed�� ������ �׻� ���� ���� ���� => ��� Ÿ���� ������ �ʿ� ����
                }
            }
        }
        lastPos = playerPos;
    }

    public int GetNoise(Vector2 pos)
    {
        int result;
        float height = Mathf.Clamp01(Mathf.PerlinNoise(seed + pos.x * 0.9f + 0.01f, seed + pos.y * 0.9f + 0.01f));
        result = Mathf.FloorToInt(height * tiles.Count);
        result = Mathf.Clamp(result, 0, tiles.Count - 1);
        return result;
    }

}