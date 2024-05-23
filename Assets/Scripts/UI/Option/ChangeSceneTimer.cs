using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneTimer : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public AudioSource bgmAudioSource; // BGM을 재생하는 AudioSource

    private void Start()
    {
        // 만약 BGM AudioSource가 null이면, 현재 게임 오브젝트에서 찾아봅니다.
        if (bgmAudioSource == null)
        {
            bgmAudioSource = GetComponent<AudioSource>();
        }

        // BGM 재생 시작
        bgmAudioSource.Play();
    }

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            // BGM 정지
            bgmAudioSource.Stop();

            // 씬 변경
            SceneManager.LoadScene(sceneName);
        }
    }
}
