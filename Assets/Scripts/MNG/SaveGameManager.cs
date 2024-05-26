using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class SaveGameManager : MonoBehaviour
{
    public GameObject player;
    public QuestManager questManager;
    public TextMeshProUGUI QuestTalk; // Text 형식 사용을 위한 선언
    public GameObject menuSet; // menuSet 변수 선언

    void Start()
    {
        Debug.Log("Inited");
        GameLoad();
        QuestTalk.text = questManager.CheckQuest();
        GameManager.Instance.m_DataManager.LoadFunc();
        GameManager.Instance.GetUnitSaveData("해태").m_AttackBehav_1 = 0;
        GameManager.Instance.GetUnitSaveData("해태").m_AttackBehav_2 = 3;
        GameManager.Instance.GetUnitSaveData("해태").m_AttackBehav_3 = 2;
        GameManager.Instance.GetUnitSaveData("해태").m_isCaptured = true;
        GameManager.Instance.m_DataManager.LoadPlayerUnits();


    }

    // 게임을 저장하는 함수
    public void SaveGame()
    {

        // 플레이어의 현재 위치 저장
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);

        // 퀘스트 진행 상황 저장
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);

        GameManager.Instance.SaveALLPlayerUnit();
        SavePlayerUnits();



        // 인벤토리 저장
        SaveInventory();
        // PlayerPrefs 저장
        PlayerPrefs.Save();

        menuSet.SetActive(false);
        GameManager.Instance.SetGameState(GameManager.GameState.INPROGRESS);
    }
    private void OnDestroy()
    {
        //SavePlayerUnits();
    }

    // 플레이어 위치와 퀘스트 ID의 저장 기록을 삭제하는 함수
    public void DeletePlayerData()
    {
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("QuestId");
        PlayerPrefs.DeleteKey("QuestActionIndex");
        PlayerPrefs.Save();
        Debug.Log("Player data and quest data have been deleted.");
    }


    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;

        // 인벤토리 불러오기
        LoadInventory();

    }

    #region 인벤토리 저장
    private void SaveInventory()
    {
        Dictionary<string, int> ItemSaveDic = new Dictionary<string, int>();

        foreach (Slot slot in Inventory_Controller.g_ICinstance.g_Sslot)
        {
            if (slot != null && slot.g_Ihave_item != null)
            {
                //ItemEntity.ItemStats_Save itemStats = new ItemEntity.ItemStats_Save();
                ItemSaveDic.Add(slot.g_Ihave_item.m_sItemName, slot.g_iitem_Number); //갯수만큼 넣어줌
            }
        }

        string path = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        string json = JsonConvert.SerializeObject(ItemSaveDic, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    private static void LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            Dictionary<string, int> tempDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            foreach (var item in tempDic)
            {
                GameObject entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ItemEntity"));
                entity.transform.GetComponent<ItemEntity>().m_sItemName = item.Key;
                entity.transform.GetComponent<ItemEntity>().SetItemInfo();

                Inventory_Controller.g_ICinstance.Set_GetItem(entity);
                Inventory_Controller.g_ICinstance.Check_Slot(item.Value);

                Debug.Log("Loaded Item:" + item.Key + "Amount:" + item.Value);
            }
        }
    }
    #endregion

    #region 플레이어 유닛 저장
    public void SavePlayerUnits()
    {
        string UnitNames = "";
        for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
        {
            UnitNames += GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName;
            UnitNames += ",";
        }
        string path = Application.persistentDataPath + "PlayerUnits.json";
        if (File.Exists(path))
            System.IO.File.Delete(path);
        File.WriteAllText(path, UnitNames);
        Debug.Log("PlayerUnit Save Complete " + path);
    }
    public void LoadPlayerUnits()
    {
        //GameManager.Instance.m_UnitManager.g_PlayerUnits[]

        string[] splitNames;
        string txtFileTemp;
        string path = Application.persistentDataPath + "PlayerUnits.json";
        if (File.Exists(path))
        {
            txtFileTemp = File.ReadAllText(path);
        }
        else
        {
            txtFileTemp = "해태";
        }
        splitNames = txtFileTemp.Split(',');
        Debug.Log(splitNames.Length);
        for (int i = 0; i < splitNames.Length; i++)
        {
            if (splitNames[i].Trim().Length != 0)
            {
                Debug.Log(splitNames[i]);
                GameManager.Instance.m_UnitManager.SetPlayerUnitEntityByName(splitNames[i], i);
            }

        }
    }
    #endregion
}
