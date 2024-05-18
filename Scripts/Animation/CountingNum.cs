using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingNum : MonoBehaviour
{
    public float playerMoney;

    public Text moneyLabel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Count(playerMoney, 0f));
    }


    IEnumerator Count(float target, float current)

    {

        float duration = 2f; // ī���ÿ� �ɸ��� �ð� ����. 

        float offset = (target - current) / duration;



        while (current < target)

        {

            current += offset * Time.deltaTime;

            moneyLabel.text = ((int)current).ToString();

            yield return null;

        }



        current = target;

        moneyLabel.text = ((int)current).ToString();

    }
}
