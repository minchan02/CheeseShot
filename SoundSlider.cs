using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
    float volume1; // bgm 볼륨 담당
    float volume2; // sfx 볼륨 담당
    public Slider bgmSlider;
    public Slider sfxSlider;
    public void Start()
    {
        SoundManager.instance.mixer.GetFloat("BGSound", out volume1); // 볼륨값 가져와서 슬라이더 값에 넣기
        bgmSlider.value = Mathf.Pow(10f, volume1 / 20f);
        SoundManager.instance.mixer.GetFloat("SFXVolume", out volume2);
        sfxSlider.value = Mathf.Pow(10f, volume2 / 20f);


        bgmSlider.onValueChanged.AddListener(SoundManager.instance.BGSoundVolume); // 소리 볼륨값 설정
        sfxSlider.onValueChanged.AddListener(SoundManager.instance.SFXSoundVolume);
    }
}
