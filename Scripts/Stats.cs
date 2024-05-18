using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class Stats : MonoBehaviour
{
    public static Stats instance = null;

    [BoxGroup("플레이어 정보")] public int level;

    [BoxGroup("재화")] public int money;

    [BoxGroup("노드")] public int ENodeInt;   // 언락 되어 있는 속성 노드 수
    [BoxGroup("노드")] public bool MNode;  // 메인 노드 언락

    [BoxGroup("UI")] public TextMeshProUGUI Coin; // 프롤로그 스킵

    [BoxGroup("기타")] public int stage; // 스테이지 
    [BoxGroup("기타")] public bool skip; // 프롤로그 스킵
  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance); // 씬이 변해도 제거 안되게 함

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //  if (!Coin) Coin = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        // Coin.text = money.ToString(); // Integer.toString
    }
}
