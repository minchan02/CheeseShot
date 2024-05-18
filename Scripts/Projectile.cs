using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "new Projectile", menuName = "»õ·Î¿î ÃÑ¾Ë ÇÁ·ÎÇÊ »ý¼º")]
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
