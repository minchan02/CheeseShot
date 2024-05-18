
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup bgmGroup;
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance); // 씬이 변해도 제거 안되게 함
            BgSoundPlay(bglist[0]); // 첫번째 실행 됬을 때만 플레이 => 나중에 수정해주어야함
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬로딩 될 때마다 체크
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
       
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        switch (arg0.name)
        {
            case "FirstLoad":
                BgSoundPlay(bglist[0]);
                break;
            case "MainMenu":
                BgSoundPlay(bglist[1]);
                break;
            case "game":
                BgSoundPlay(bglist[2]);
                break;
            case "Shop":
                BgSoundPlay(bglist[2]);
                break;
            case "Loading":
                BgSoundPlay(bglist[0]);
                break;
        }

    }

    
    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGSound", Mathf.Log10(val) * 20f); // 배경음 소리 크기 조정 Mathf.Log10(val) * 20f
    }

    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(val) * 20f); // 배경음 소리 크기 조정
    } 

    public void SFXPlay(AudioClip clip) // 효과음
    {
        GameObject go = new GameObject("SFXSound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.outputAudioMixerGroup = sfxGroup; // 오디오 믹서 추가 - 이름은 그룹에서 찾아 추가 -> 현재 오류!
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip) // Bgm
    {
        bgSound.outputAudioMixerGroup = bgmGroup; 
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }
}
