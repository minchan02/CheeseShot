using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CashEditor : MonoBehaviour
{

    public int newCash;

    [Button("재화 지정 (New Cash에 값 입력 / 플레이 중에만)", EButtonEnableMode.Playmode)]
    public void SetCash()
    {
        DBManager.DB.cash = newCash;
    }

}
