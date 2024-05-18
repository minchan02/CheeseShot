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

    


    //----------------- �ɼ� ����---------------//
    public void InverseSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void ChangeJoyButtonImage()
    {
        if (joystick.GetFix()) // ������ true�� �� ON�̹�����
        {
            FixJoystick.image.sprite = FixON;
        }

        else
        {
            FixJoystick.image.sprite = FixOFF;
        }
    }

}
