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
    int previousBgmIndex; // 이전 BGM 인덱스 저장

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

        // 이전 BGM 인덱스 저장
        previousBgmIndex = g_currentBgmIndex;

        // 기존 배경음 중지
        StopBgm();

        g_currentBgmIndex = bgmIndex; // 현재 배경음 인덱스 변경
        g_bgmPlayer.clip = g_bgmClip[g_currentBgmIndex]; // 배경음 플레이어에 새로운 클립 설정
        g_bgmPlayer.Play(); // 새로운 배경음 재생
    }
    void OnEnable()
    {
        // 씬이 로드될 때와 언로드될 때의 이벤트 핸들러 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        // 씬이 로드될 때와 언로드될 때의 이벤트 핸들러 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // 씬이 로드될 때 호출되는 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬의 이름이 "BattleScene"인 경우 배틀 BGM으로 전환
        if (scene.name == "BattleScene")
        {
            SwitchBgm(1);
        }
    }

    // 씬이 언로드될 때 호출되는 메서드
    void OnSceneUnloaded(Scene scene)
    {
        // 언로드된 씬의 이름이 "BattleScene"인 경우 이전 BGM으로 전환
        if (scene.name == "BattleScene")
        {
            SwitchBgm(previousBgmIndex);
        }
    }
}

