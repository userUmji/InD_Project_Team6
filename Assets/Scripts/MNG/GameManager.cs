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
    public DataAssetManager m_DataManager;
    //플레이어의 유닛을 관리할 유닛메니저
    public UnitManager m_UnitManager;
    //월드씬의 캔버스
    private GameObject Canvas_WorldScene;
    //게임의 진행 상황 (초기화, 진행중, 대화중, 전투중, 일지정지)
    public enum GameState { INIT, INPROGRESS, DIALOG,  PORTAL ,BATTLE, PAUSE };
    public GameState g_GameState;
    private GameObject g_InventoryGO;
    private GameObject g_DictionaryGO;
    public List<SOAttackBase> Skills;
    public int[] g_iReqExp;
    public int g_Season;

    public string g_sEnemyBattleUnit;
    public int g_iEnemyBattleLvl;

    public enum Action { CANCLE, ATTACK, ITEM, CHANGE, RUN }
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
        m_DataManager.LoadFunc();
        InitExp();
        
       // m_UnitManager.SetPlayerUnitEntityByName("해태", 0);
        GetUnitSaveData("해태").m_AttackBehav_1 = 0;
        GetUnitSaveData("해태").m_AttackBehav_2 = 3;
        GetUnitSaveData("해태").m_AttackBehav_3 = 2;
        GetUnitSaveData("해태").m_isCaptured = true;
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
    public UnitTable.UnitStats_Save GetUnitSaveData(string className)
    {
        return m_DataManager.GetUnitSaveData(className);
    }
    public void LoadBattleScene(string enemyBattleUnit, int lvl)
    {
        InitGOs();
        g_GameState = GameState.BATTLE;
        




        AsyncOperation SceneOper = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
        g_sEnemyBattleUnit = enemyBattleUnit;
        g_iEnemyBattleLvl = lvl;
        GetWorldCanvasGO().SetActive(false);
        SceneOper.allowSceneActivation = true; 
    }
    IEnumerator LoadBattleScene()
    {
        int waitTime = 0;
        while(true) { 


            yield return new WaitForEndOfFrame();
        }
        
        
    }


    public void SaveALLPlayerUnit()
    {
        for(int i = 0; i<m_UnitManager.CheckUnitAmount();i++)
        {
            UnitEntity unitEntity = m_UnitManager.g_PlayerUnits[i].transform.GetComponent<UnitEntity>();
            m_DataManager.SaveByUnit(unitEntity.m_sUnitName, unitEntity);
        }
        m_DataManager.SaveFunc_ALL();
    }
    public void SavePlayerUnit(string name, UnitEntity entity)
    {
        m_DataManager.SaveByUnit(name, entity);
    }
    // 게임 상태를 설정하는 메서드
    public void SetGameState(GameState state)
    {
        g_GameState = state;
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
    public int CompareType(Type SkillType, Type UnitType)
    {
        int isDouble = 0;
        if (UnitType == Type.GODBEAST ||SkillType == Type.GODBEAST)
            isDouble = 0;
        else if (SkillType - UnitType == -1 || SkillType - UnitType == 3)
            isDouble = 1;
        else if (SkillType - UnitType == 1 || SkillType - UnitType == -3)
            isDouble = 2;
        else
            isDouble = 0;
        return isDouble;
    }
    public string TypeToString(Type skillType)
    {
        switch(skillType)
        {
            case Type.FIRE :
                return "불";
            case Type.GHOST:
                return "귀신";
            case Type.GODBEAST:
                return "신수";
            case Type.ICE:
                return "얼음";
            case Type.MONSTER:
                return "괴수";
        }
        return null;
    }
    public GameObject GetInventoryGO()
    {
        if(g_InventoryGO == null)
            g_InventoryGO = GameObject.Find("Inventory");

        return g_InventoryGO;
    }
    public GameObject GetDictionaryGO()
    {
        if (g_DictionaryGO == null)
            g_DictionaryGO = GameObject.Find("Map").transform.GetComponent<WorldMapController>().Dic;

            
        return g_DictionaryGO;
    }
    public GameObject GetWorldCanvasGO()
    {
        if(Canvas_WorldScene == null)
        {
           Canvas_WorldScene =  GameObject.Find("Canvas_World");
        }
        return Canvas_WorldScene;
    }
    #endregion
    public void LoseChangeScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void InitGOs()
    {
        GetWorldCanvasGO();
        GetDictionaryGO();
        GetInventoryGO();
    }

    private void InitExp()
    {
        g_iReqExp = new int[51];
        g_iReqExp[0] = 0;
        g_iReqExp[1] = 1000;
        g_iReqExp[2] = 1749;
        g_iReqExp[3] = 2435;
        g_iReqExp[4] = 3073;
        g_iReqExp[5] = 3669;
        g_iReqExp[6] = 4227;
        g_iReqExp[7] = 4752;
        g_iReqExp[8] = 5245;
        g_iReqExp[9] = 5710;
        g_iReqExp[10] = 6150;
        g_iReqExp[11] = 6567;
        g_iReqExp[12] = 6964;
        g_iReqExp[13] = 7342;
        g_iReqExp[14] = 7703;
        g_iReqExp[15] = 8049;
        g_iReqExp[16] = 8381;
        g_iReqExp[17] = 8700;
        g_iReqExp[18] = 9008;
        g_iReqExp[19] = 9305;
        g_iReqExp[20] = 9592;
        g_iReqExp[21] = 9870;
        g_iReqExp[22] = 10140;
        g_iReqExp[23] = 10402;
        g_iReqExp[24] = 10657;
        g_iReqExp[25] = 10905;
        g_iReqExp[26] = 11147;
        g_iReqExp[27] = 11382;
        g_iReqExp[28] = 11612;
        g_iReqExp[29] = 11836;
        g_iReqExp[30] = 12055;
        g_iReqExp[31] = 12268;
        g_iReqExp[32] = 12477;
        g_iReqExp[33] = 12681;
        g_iReqExp[34] = 12880;
        g_iReqExp[35] = 13075;
        g_iReqExp[36] = 13266;
        g_iReqExp[37] = 13452;
        g_iReqExp[38] = 13635;
        g_iReqExp[39] = 13815;
        g_iReqExp[40] = 13991;
        g_iReqExp[41] = 14164;
        g_iReqExp[42] = 14333;
        g_iReqExp[43] = 14499;
        g_iReqExp[44] = 14662;
        g_iReqExp[45] = 14821;
        g_iReqExp[46] = 14978;
        g_iReqExp[47] = 15131;
        g_iReqExp[48] = 15282;
        g_iReqExp[49] = 15430;
        g_iReqExp[50] = 15575;
    }
}