using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
    float volume1; // bgm ���� ���
    float volume2; // sfx ���� ���
    public Slider bgmSlider;
    public Slider sfxSlider;
    public void Start()
    {
        SoundManager.instance.mixer.GetFloat("BGSound", out volume1); // ������ �����ͼ� �����̴� ���� �ֱ�
        bgmSlider.value = Mathf.Pow(10f, volume1 / 20f);
        SoundManager.instance.mixer.GetFloat("SFXVolume", out volume2);
        sfxSlider.value = Mathf.Pow(10f, volume2 / 20f);


        bgmSlider.onValueChanged.AddListener(SoundManager.instance.BGSoundVolume); // �Ҹ� ������ ����
        sfxSlider.onValueChanged.AddListener(SoundManager.instance.SFXSoundVolume);
    }
}
