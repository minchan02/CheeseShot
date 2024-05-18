using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType
{
    NormalAtk, // 일반공격
    AttributeAtk, // 속성공격 
    MainAtk, // 메인공격
    Etc // 기타
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
}