using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TalkManager m_TalkManager;
    public GameObject m_talkPanel;
    public Image m_portraitImg;
    public Text m_talkText;
    public GameObject m_scanObject;
    public bool isAct;
    public int talkIndex;

    // 상호작용 
    public void Act(GameObject scanObj)
    {
        // 스캔된 오브젝트를 저장하고 오브젝트 데이터를 가져옴
        m_scanObject = scanObj;
        ObjData objData = m_scanObject.GetComponent<ObjData>();
        // 대화 함수 호출
        Talk(objData.id, objData.isNpc);

        // 대화 패널 활성화
        m_talkPanel.SetActive(isAct);
    }

    #region 대화 관련
    void Talk(int id, bool isNpc)
    {
        // 대화 데이터 가져오기
        string talkData = m_TalkManager.GetTalk(id, talkIndex);

        // 대화 데이터가 없으면 상호작용 종료
        if (talkData == null)
        {
            isAct = false;
            talkIndex = 0;
            return;
        }

        // NPC인 경우
        if (isNpc)
        {
            // 대화 텍스트 설정
            m_talkText.text = talkData.Split(':')[0];
            // 초상화 이미지 설정
            m_portraitImg.sprite = m_TalkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            m_portraitImg.color = new Color(1, 1, 1, 1); // 초상화 이미지 투명도 설정
        }
        else // NPC가 아닌 경우
        {
            // 대화 텍스트 설정
            m_talkText.text = talkData;
            // 초상화 이미지 투명하게 설정
            m_portraitImg.color = new Color(1, 1, 1, 0);
        }

        // 상호작용 활성화 및 대화 인덱스 증가
        isAct = true;
        talkIndex++;
    }
    #endregion
}
