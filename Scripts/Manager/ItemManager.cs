using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Player player;
    public static ItemManager instance; // 다른 클래스에서 접근 가능하게 함
    public List<Item> itemDB = new List<Item>(); // 아이템 리스트
    public int[] itemidx = new int[] { 0, 0, 0, 0, 0, 0, 0};

    public GameObject wasteBtn;
    public GameObject OkBtn;
    public GameObject CancelBtn;
    public GameObject ChangeSkill;

    public Image LeftImage;
    public Image RightImage;
    
    private int Lnum;
    private int Rnum;
    private int curidx = -1;
    private int PosUp = 50;

    private RectTransform LPos;
    private RectTransform RPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LPos = LeftImage.GetComponent<RectTransform>();
        RPos = RightImage.GetComponent<RectTransform>();
        // .anchoredPosition.y;
    }

    public void SetItem()   // 아이템 설정 초기화
    {
        ChangeSkill.SetActive(true);
        Lnum = Random.Range(0, 7);
        Lnum = FindItemNum(Lnum);
        Rnum = Random.Range(0, 7);
        Rnum = FindItemNum(Rnum);

        LeftImage.sprite = itemDB[Lnum].itemImage;
        RightImage.sprite = itemDB[Rnum].itemImage;
       // Ltext.text = itemDB[Lnum].itemExplain;
       // Rtext.text = itemDB[Rnum].itemExplain;
    }

    public int FindItemNum(int i)
    {
        while (true)
        {
            if (itemidx[i] == 1 || Lnum == i)  // 이미 아이템이 있음
            {
                i = Random.Range(0, 7);
            }

            else if (itemidx[i] == 0 && Lnum != i)   // 없는 아이템만 나옴
            {
                return i;
            }
        }
    }
    public void SetUI(int idx)
    {

        // ChangeSkill.SetActive(false);
        if (wasteBtn.activeSelf == true)
        {
            wasteBtn.SetActive(false);
            OkBtn.SetActive(true);
            CancelBtn.SetActive(true);
        }

        if(curidx != -1)
        {
            return;
        }

        if (idx == 0)
        {
            curidx = 0;
            Color color = RightImage.color;
            color.a = 0.5f;
            RightImage.color = color;
            LeftImage.rectTransform.sizeDelta = new Vector2(400, 400);
            LPos.anchoredPosition = new Vector2(LPos.anchoredPosition.x, LPos.anchoredPosition.y + PosUp);
        }

        else if (idx == 1)
        {
            curidx = 1;
            Color color = LeftImage.color;
            color.a = 0.5f;
            LeftImage.color = color;
            RightImage.rectTransform.sizeDelta = new Vector2(400, 400);
            RPos.anchoredPosition = new Vector2(RPos.anchoredPosition.x, RPos.anchoredPosition.y + PosUp);
        }
        
    }

    public void ApplyItem()
    {
        if (curidx == 0)
        {
            itemidx[Lnum] = 1;
            player.SetNode(itemDB[Lnum]);   // 왼쪽
            Back();
            LPos.anchoredPosition = new Vector2(LPos.anchoredPosition.x, LPos.anchoredPosition.y - PosUp);
        }
        else if (curidx == 1)
        {
            itemidx[Rnum] = 1;
            player.SetNode(itemDB[Rnum]);   // 오른쪽
            Back();
            RPos.anchoredPosition = new Vector2(RPos.anchoredPosition.x, RPos.anchoredPosition.y - PosUp);
        }
        else
        {
            return;
        }
    }

    public void Cancel()
    {
        if (curidx == 0)
        {
            Color color = RightImage.color;
            color.a = 1f;
            RightImage.color = color;
            LeftImage.rectTransform.sizeDelta = new Vector2(300, 300);
            LPos.anchoredPosition = new Vector2(LPos.anchoredPosition.x, LPos.anchoredPosition.y - PosUp);
        }

        else if (curidx == 1)
        {
            Color color = LeftImage.color;
            color.a = 1f;
            LeftImage.color = color;
            RightImage.rectTransform.sizeDelta = new Vector2(300, 300);
            RPos.anchoredPosition = new Vector2(RPos.anchoredPosition.x, RPos.anchoredPosition.y - PosUp);
        }

        curidx = -1;

        wasteBtn.SetActive(true);
        OkBtn.SetActive(false);
        CancelBtn.SetActive(false);
    }

    public void Back()
    {
        curidx = -1;

        wasteBtn.SetActive(true);
        OkBtn.SetActive(false);
        CancelBtn.SetActive(false);

        Color colorA = LeftImage.color;
        colorA.a = 1f;
        LeftImage.color = colorA;
        LeftImage.rectTransform.sizeDelta = new Vector2(300, 300);

        Color colorB = RightImage.color;
        colorB.a = 1f;
        RightImage.color = colorB;
        RightImage.rectTransform.sizeDelta = new Vector2(300, 300);

        ChangeSkill.SetActive(false);
        
        
    }
    
}
