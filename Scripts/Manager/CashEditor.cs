using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CashEditor : MonoBehaviour
{

    public int newCash;

    [Button("��ȭ ���� (New Cash�� �� �Է� / �÷��� �߿���)", EButtonEnableMode.Playmode)]
    public void SetCash()
    {
        DBManager.DB.cash = newCash;
    }

}
