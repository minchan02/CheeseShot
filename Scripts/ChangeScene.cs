using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{


    private void Awake()
    {

    }

    public void ToFirst()    // 처음화면으로
    {
        LoadingSceneController.LoadScene("FirstLoad");
    }
    public void ToMain() // 메인으로
    {
       LoadingSceneController.LoadScene("MainMenu");
    }
    public void ToSelectStage()  // 스테이지 선택
    {
        LoadingSceneController.LoadScene("SelectStage");
    }

    public void ToStage1()    // 1스테이지
    {
        LoadingSceneController.LoadScene("game");
    }
    
    public void ToShop() // 상점으로
    {
        LoadingSceneController.LoadScene("Shop");
    }

    public void ToShop2()
    {
        LoadingSceneController.LoadScene("Shop2");
    }
}
