using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // 게임 일시 정지 UI를 참조할 GameObject 변수
    public GameObject PauseUI;

    // 게임이 일시 정지되었는지를 나타내는 불리언 변수
    private bool paused = false;

    // Start 함수는 처음 한 번 호출됩니다.
    void Start()
    {
        // 시작 시 일시 정지 UI를 비활성화합니다.
        PauseUI.SetActive(false);
    }

    // Update 함수는 매 프레임마다 호출됩니다.
    void Update()
    {
        // "Pause" 입력이 감지되면
        if (Input.GetButtonDown("Pause"))
        {
            // 일시 정지 상태를 반전시킵니다.
            paused = !paused;
        }

        // 만약 게임이 일시 정지 상태라면
        if (paused)
        {
            // 일시 정지 UI를 활성화하고 시간을 멈춥니다.
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else // 일시 정지 상태가 아니라면
        {
            // 일시 정지 UI를 비활성화하고 시간을 정상적으로 진행합니다.
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // Resume 함수는 재개 버튼을 눌렀을 때 호출됩니다.
    public void Resume()
    {
        // 일시 정지 상태를 반전시킵니다.
        paused = !paused;
    }
}
