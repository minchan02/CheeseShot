using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "new Enemy", menuName = "���ο� �� ������ ����")]
public class Enemy : ScriptableObject
{

    [Header("Visual"), ShowAssetPreview]
    public Material material;

    [Header("Resources")]
    public GameObject prefab;

    [Header("Specs")]
    public long price; // óġ �� ���� ��ȭ�� ��

}

