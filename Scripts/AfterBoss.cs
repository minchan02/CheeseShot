using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBoss : MonoBehaviour
{
    public static AfterBoss instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject setAfterClear;
    public GameObject setAfterFail;

    public void ClearMiddleStage()
    {
        setAfterClear.SetActive(true);
    }

    public void DisableClearSet()
    {
        setAfterClear.SetActive(false);
    }
    
    public void failStage()
    {
        setAfterFail.SetActive(true);
    }

    public void DisablefailSet()
    {
        setAfterFail.SetActive(false);
    }


}
