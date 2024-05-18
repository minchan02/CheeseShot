using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Joystick joystick;


    [Foldout("UI")] public GameObject settingsPanel;

    [Foldout("Joystick")] public Button FixJoystick;
    [Foldout("Joystick")] public Sprite FixON;
    [Foldout("Joystick")] public Sprite FixOFF;

    


    //----------------- 옵션 설정---------------//
    public void InverseSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void ChangeJoyButtonImage()
    {
        if (joystick.GetFix()) // 고정이 true일 시 ON이미지로
        {
            FixJoystick.image.sprite = FixON;
        }

        else
        {
            FixJoystick.image.sprite = FixOFF;
        }
    }

}
