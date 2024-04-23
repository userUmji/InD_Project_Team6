using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    // 배경음(BGM) 관련 변수들
    [Header("#BGM")]
    public AudioClip[] g_bgmClip;
    public AudioMixerGroup g_musicMixerGroup;
    AudioSource g_bgmPlayer;
    int g_currentBgmIndex;

    // 효과음(SFX) 관련 변수들
    [Header("#SFX")]
    public AudioClip[] g_sfxClip;
    public AudioMixerGroup g_sfxMixerGroup;
    public int g_channels;
    AudioSource[] g_sfxPlayers;
    int g_channelIndex;

    // 효과음에 대한 열거형
    public enum Sfx { Click1, Click2, Item, Select, Talk }

    void Awake()
    {
        _instance = this;
        Init();
    }

    void Start()
    {
        _instance.SwitchBgm(0);
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        g_bgmPlayer = bgmObject.AddComponent<AudioSource>();
        g_bgmPlayer.playOnAwake = false;
        g_bgmPlayer.loop = true;
        g_bgmPlayer.outputAudioMixerGroup = g_musicMixerGroup; // 배경음 AudioSource에 배경음의 믹서 그룹 설정
        g_bgmPlayer.clip = g_bgmClip[g_currentBgmIndex]; // 현재 재생 중인 배경음 클립 설정

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        g_sfxPlayers = new AudioSource[g_channels]; // 효과음 플레이어 배열 생성

        for (int i = 0; i < g_sfxPlayers.Length; i++)
        {
            g_sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            g_sfxPlayers[i].playOnAwake = false;
            g_sfxPlayers[i].outputAudioMixerGroup = g_sfxMixerGroup; // 효과음 AudioSource에 효과음의 믹서 그룹 설정
        }
    }

    // 특정 효과음을 재생하는 메서드
    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < g_sfxPlayers.Length; i++)
        {
            int loopIndex = (i + g_channelIndex) % g_sfxPlayers.Length;

            if (g_sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            g_channelIndex = loopIndex;
            g_sfxPlayers[loopIndex].clip = g_sfxClip[(int)sfx];
            g_sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // 배경음 재생 중지 메서드
    public void StopBgm()
    {
        if (g_bgmPlayer.isPlaying)
        {
            g_bgmPlayer.Stop();
        }
    }

    // 특정 배경음을 전환하는 메서드
    public void SwitchBgm(int bgmIndex)
    {
        if (bgmIndex < 0 || bgmIndex >= g_bgmClip.Length)
        {
            return;
        }

        // 기존 배경음 중지
        StopBgm();

        g_currentBgmIndex = bgmIndex; // 현재 배경음 인덱스 변경
        g_bgmPlayer.clip = g_bgmClip[g_currentBgmIndex]; // 배경음 플레이어에 새로운 클립 설정
        g_bgmPlayer.Play(); // 새로운 배경음 재생
    }

    // 배틀씬 로드시 Bgm 중지
     //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
        //if (scene.name == "BattleScene")
       // {
           // _instance.StopBgm();
           // SceneManager.sceneLoaded -= OnSceneLoaded; // 콜백 함수 제거
        //}
    //}

}
