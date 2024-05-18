using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //타일맵 시스템을 이용하기 위해
using NaughtyAttributes;

public class MapManager : MonoBehaviour
{

    public static MapManager instance;

    [Foldout("외부 클래스")] public Tilemap tilemap; //편의를 위해 변수 네이밍 통일

    [BoxGroup("설정")] public int seed; //더 편하게 저장 가능
    [BoxGroup("설정")] public Vector2Int amount = new Vector2Int(1, 2); //화면에 보여질 타일의 갯수 / 가로 Size.x*2, 세로 Size.y*2 만큼 화면에 보이게 됨
    [BoxGroup("설정")] public float tilemapScale; //Grid 오브젝트의 크기
    [BoxGroup("설정")] public float updateRate; //타일을 몇초마다 업데이트 할 것인가?
    [BoxGroup("설정")] public List<TileBase> tiles; //표시할 타일맵 리스트. TileBase[] 와 유사

    [Foldout("디버그")] [ReadOnly] public Vector2Int lastPos;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        seed = Random.Range(-10000, 10000); //랜덤 시드 고르기 / 로드시 저장된 시드 사용
        InvokeRepeating("UpdateTiles", 0, updateRate); //성능 향상을 위해 UpdateTiles()를 updateRate초 마다 실행
    }

    void Update()
    {
        
    }

    public void UpdateTiles()
    {
        Vector2Int playerPos = Vector2Int.RoundToInt(Player.instance.transform.position / tilemapScale);
        if (playerPos != lastPos)
        {
            tilemap.ClearAllTiles(); //모든 타일 삭제, 쓰이지 않는 타일 제거 목적
            for (int x = -amount.x; x < amount.x; x++)
            {
                for (int y = -amount.y; y < amount.y; y++)
                {
                    Vector2Int pos = playerPos + new Vector2Int(x, y); //캐싱 / 같은 계산이 여러번 필요한 경우
                    int height = GetNoise(pos);
                    tilemap.SetTile((Vector3Int)pos, tiles[height]); //pos에 타일 배치 / Seed가 같으면 항상 같은 값이 나옴 => 모든 타일을 저장할 필요 없음
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