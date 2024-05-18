using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class Stats : MonoBehaviour
{
    public static Stats instance = null;

    [BoxGroup("�÷��̾� ����")] public int level;

    [BoxGroup("��ȭ")] public int money;

    [BoxGroup("���")] public int ENodeInt;   // ��� �Ǿ� �ִ� �Ӽ� ��� ��
    [BoxGroup("���")] public bool MNode;  // ���� ��� ���

    [BoxGroup("UI")] public TextMeshProUGUI Coin; // ���ѷα� ��ŵ

    [BoxGroup("��Ÿ")] public int stage; // �������� 
    [BoxGroup("��Ÿ")] public bool skip; // ���ѷα� ��ŵ
  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance); // ���� ���ص� ���� �ȵǰ� ��

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
