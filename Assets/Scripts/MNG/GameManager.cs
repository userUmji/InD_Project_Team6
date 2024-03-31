using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    //싱글턴 인스턴스 존재를 확인하는 용도
    private static GameManager _instance;
    public UnitTable m_AssetUnitTable;
    public ItemTable m_AssetItemTable;
    DataAssetManager m_DataManager;
    public UnitManager m_UnitManager;
    public TalkManager m_TalkManager;
    public GameObject m_talkPanel;
    public Image m_portraitImg;
    public Text m_talkText;
    public GameObject m_scanObject;
    public bool isAct;
    public int talkIndex;

    //싱글턴 구현
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //지정된 인스턴스가 없다면 지정, 이미 존재한다면 대체
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //데이터메니저 Init을 실행시킴

        Init();

    }
    private void Init()
    {
        m_DataManager = new DataAssetManager();
        m_DataManager.Init(m_AssetUnitTable, m_AssetItemTable);
        m_UnitManager = new UnitManager();
        m_UnitManager.SetPlayerUnitEntityByName("개굴닌자", 0);
        m_UnitManager.SetPlayerUnitEntityByName("개구마루", 1);
        m_UnitManager.SetPlayerUnitEntityByName("개굴반장", 2);

        Debug.Log(m_UnitManager.GetUnitEntity(0).transform.GetComponent<UnitEntity>().m_sUnitName);
        Debug.Log(m_UnitManager.GetUnitEntity(1).transform.GetComponent<UnitEntity>().m_sUnitName);
        Debug.Log(m_UnitManager.GetUnitEntity(2).transform.GetComponent<UnitEntity>().m_sUnitName);
    }
    //유닛 데이터를 가져오는 기능
    public UnitTable.UnitStats GetUnitData(string className)
    {
        return m_DataManager.GetUnitData(className);
    }
    public ItemTable.ItemStats GetItemData(string className)
    {
        return m_DataManager.GetItemData(className);
    }

    #region 타입 관련
    public enum Type
    {
        GODBEAST,
        FIRE,
        ICE,
        MONSTER,
        GHOST
    }
    public enum Action { NULL, ATTACK, ITEM, CHANGE, RUN }
    public int CompareType(Type SkillType, Type UnitType)
    {
        int isDouble = 0;
        if (UnitType == Type.GODBEAST)
            isDouble = 0;
        else if (SkillType - UnitType == -1 || SkillType - UnitType == 3)
            isDouble = 1;
        else if (SkillType - UnitType == 1 || SkillType - UnitType == -3)
            isDouble = 2;
        else
            isDouble = 0;
        return isDouble;
    }
    #endregion

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