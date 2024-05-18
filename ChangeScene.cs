using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{


    private void Awake()
    {

    }

    public void ToFirst()    // ó��ȭ������
    {
        LoadingSceneController.LoadScene("FirstLoad");
    }
    public void ToMain() // ��������
    {
       LoadingSceneController.LoadScene("MainMenu");
    }
    public void ToSelectStage()  // �������� ����
    {
        LoadingSceneController.LoadScene("SelectStage");
    }

    public void ToStage1()    // 1��������
    {
        LoadingSceneController.LoadScene("game");
    }
    
    public void ToShop() // ��������
    {
        LoadingSceneController.LoadScene("Shop");
    }

    public void ToShop2()
    {
        LoadingSceneController.LoadScene("Shop2");
    }
}
