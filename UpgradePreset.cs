using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "��ȭ ������", fileName = "UpgradePreset")]
public class UpgradePreset : ScriptableObject
{

    [Header("���� ĭ���� �ʱⰪ(0������ �� ��)��, ���� ĭ���� ������(���)�� ���´�")]
    public Vector2Int cashPerUpgrade; // x�� 1������ �� ��, y�� ������ (%)
    public Vector2Int cashPerKill;
    public Vector2Int powerPerUpgrade;
    public Vector2Int healthPerUpgrade;

}
