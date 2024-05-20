using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class SaveGameManager : MonoBehaviour
{
    public GameObject player;
    public QuestManager questManager;
    public TextMeshProUGUI QuestTalk; // Text 형식 사용을 위한 선언

    public GameObject menuSet; // menuSet 변수 선언

    void Start()
    {
        player = GameObject.Find("Player");
        GameLoad();
        QuestTalk.text = questManager.CheckQuest();
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



        // 인벤토리 저장
        SaveInventory();
        // PlayerPrefs 저장
        PlayerPrefs.Save();

        menuSet.SetActive(false);
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
                ItemEntity.ItemStats_Save itemStats = new ItemEntity.ItemStats_Save();
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
}
