using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "new Projectile", menuName = "새로운 총알 프로필 생성")]
public class Projectile : ScriptableObject
{

    [Header("Visual"), ShowAssetPreview]
    public Material material;
    public float angle;

    [Header("Specs")]
    public short damage;
    public float speed;
    public Vector3 scale;

}
