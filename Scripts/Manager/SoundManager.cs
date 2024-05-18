
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
            DontDestroyOnLoad(instance); // ���� ���ص� ���� �ȵǰ� ��
            BgSoundPlay(bglist[0]); // ù��° ���� ���� ���� �÷��� => ���߿� �������־����
            SceneManager.sceneLoaded += OnSceneLoaded; // ���ε� �� ������ üũ
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
        mixer.SetFloat("BGSound", Mathf.Log10(val) * 20f); // ����� �Ҹ� ũ�� ���� Mathf.Log10(val) * 20f
    }

    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(val) * 20f); // ����� �Ҹ� ũ�� ����
    } 

    public void SFXPlay(AudioClip clip) // ȿ����
    {
        GameObject go = new GameObject("SFXSound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.outputAudioMixerGroup = sfxGroup; // ����� �ͼ� �߰� - �̸��� �׷쿡�� ã�� �߰� -> ���� ����!
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
