using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "new Enemy", menuName = "새로운 적 프로필 생성")]
public class Enemy : ScriptableObject
{

    [Header("Visual"), ShowAssetPreview]
    public Material material;

    [Header("Resources")]
    public GameObject prefab;

    [Header("Specs")]
    public long price; // 처치 시 얻을 재화의 양

}

