using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "강화 프리셋", fileName = "UpgradePreset")]
public class UpgradePreset : ScriptableObject
{

    [Header("좌측 칸에는 초기값(0레벨일 때 값)을, 우측 칸에는 증가율(배수)을 적는다")]
    public Vector2Int cashPerUpgrade; // x가 1레벨일 때 값, y가 증가율 (%)
    public Vector2Int cashPerKill;
    public Vector2Int powerPerUpgrade;
    public Vector2Int healthPerUpgrade;

}
