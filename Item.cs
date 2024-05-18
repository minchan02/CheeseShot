using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType
{
    NormalAtk, // �Ϲݰ���
    AttributeAtk, // �Ӽ����� 
    MainAtk, // ���ΰ���
    Etc // ��Ÿ
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
}