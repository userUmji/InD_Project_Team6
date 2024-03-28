using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene 관련 기능을 사용하기 위한 네임스페이스 추가

[System.Serializable]
public class Dialogue
{
    [TextArea]
    public string dialogue; // 대화 내용을 저장하는 변수
    public Sprite cg; // 대화 중에 표시할 이미지를 저장하는 변수
    public bool isLeft; // 이미지를 왼쪽에 표시할지 여부를 나타내는 변수
}

public class Dialog : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteStandingCG; // 캐릭터 이미지를 표시하는 SpriteRenderer
    [SerializeField] private SpriteRenderer m_spriteDialogueBox; // 대화 상자를 표시하는 SpriteRenderer
    [SerializeField] private Text m_txtDialogue; // 대화 텍스트를 표시하는 Text 컴포넌트
    [SerializeField] private Vector2 m_leftPosition; // 왼쪽에 표시할 좌표
    [SerializeField] private Vector2 m_rightPosition;// 오른쪽에 표시할 좌표

    [SerializeField] private Dialogue[] m_dialogue; // 대화 정보를 담고 있는 배열


    private bool m_isDialogue = false; // 대화 중인지 여부를 나타내는 플래그
    private int m_count = 0; // 현재 대화 인덱스를 저장하는 변수

    // 대화 시작 메서드
    public void ShowDialogue()
    {
        OnOff(true); // 대화 상자, 캐릭터 이미지, 대화 텍스트를 화면에 표시
        m_count = 0; // 대화 인덱스 초기화
        NextDialogue(); // 첫 번째 대화로 이동
    }

    // UI 요소를 활성화 또는 비활성화하는 메서드
    private void OnOff(bool _flag)
    {
        m_spriteDialogueBox.gameObject.SetActive(_flag);
        m_spriteStandingCG.gameObject.SetActive(_flag);
        m_txtDialogue.gameObject.SetActive(_flag);
        m_isDialogue = _flag;
    }

    // 다음 대화로 이동하는 메서드
    private void NextDialogue()
    {
        m_txtDialogue.text = m_dialogue[m_count].dialogue; // 대화 텍스트 업데이트
        m_spriteStandingCG.sprite = m_dialogue[m_count].cg; // 캐릭터 이미지 업데이트

        // 이미지 위치를 설정
        if (m_dialogue[m_count].isLeft)
        {
            m_spriteStandingCG.transform.localPosition = m_leftPosition; // 왼쪽에 표시
        }
        else
        {
            m_spriteStandingCG.transform.localPosition = m_rightPosition; // 오른쪽에 표시
        }

        m_count++; // 다음 대화로 이동
    }

    void Update()
    {
        // 게임이 퍼즈 상태인지 확인
        if (Time.timeScale != 0)
        {
            // 대화 중일 때
            if (m_isDialogue)
            {
                // 마우스 왼쪽 버튼을 클릭했을 때
                if (Input.GetMouseButtonDown(0))
                {
                    // 대화가 남아있을 경우
                    if (m_count < m_dialogue.Length)
                    {
                        NextDialogue(); // 다음 대화로 이동
                    }
                    // 대화가 끝났을 경우
                    else
                    {
                        OnOff(false); // 모든 대화가 끝나면 대화 종료
                    }
                }
            }
        }
    }
}
