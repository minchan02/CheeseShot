using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Stats stat;
    void Start()
    {
        stat = GameObject.Find("BaseStats").GetComponent<Stats>();  // BaseStats ������Ʈ�� Stats ��ũ��Ʈ �ҷ��� 
    }

    public void IsMNode()
    {
        if(stat.MNode == true)
        {
            stat.MNode = false;
        }

        else
        {
            stat.MNode = true;
        }
    }
}
