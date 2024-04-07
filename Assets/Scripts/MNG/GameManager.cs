using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //싱글턴 인스턴스 존재를 확인하는 용도
    private static GameManager _instance;
    //유닛들의 데이터가 있는 테이블
    public UnitTable m_AssetUnitTable;
    //아이템의 데이터가 있는 테이블
    public ItemTable m_AssetItemTable;
    //데이터에셋을 초기화할 데이터매니저
    DataAssetManager m_DataManager;
    //플레이어의 유닛을 관리할 유닛메니저
    public UnitManager m_UnitManager;
    //월드씬의 캔버스
    public GameObject Canvas_WorldScene;
    public GameState g_GameState;
    public string g_sEnemyBattleUnit;

    public enum GameState { INIT, INPROGRESS, DIALOG, BATTLE, PAUSE };
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
        g_GameState = GameState.INIT;
        DontDestroyOnLoad(gameObject);

        Init();
    }
    private void Init()
    {
        //Data를 관리할 DataAssetManager를 생성하고 Init
        m_DataManager = new DataAssetManager();
        m_DataManager.Init(m_AssetUnitTable, m_AssetItemTable);
        //Unit을 관리할 UnitManager을 생성하고 Init
        m_UnitManager = new UnitManager();
        m_UnitManager.SetPlayerUnitEntityByName("해태", 0);
        m_UnitManager.SetPlayerUnitEntityByName("백요호", 1);
        m_UnitManager.SetPlayerUnitEntityByName("백호", 2);
        g_GameState = GameState.INPROGRESS;

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
    public void LoadBattleScene(string enemyBattleUnit)
    {
        g_GameState = GameState.BATTLE;
        AsyncOperation SceneOper = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
        g_sEnemyBattleUnit = enemyBattleUnit;
        Canvas_WorldScene.SetActive(false);
        SceneOper.allowSceneActivation = true;
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
    public enum Action { CANCLE, ATTACK, ITEM, CHANGE, RUN }
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
}