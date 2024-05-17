using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class InventorySaveLoad : MonoBehaviour
{
    public static void SaveInventory()
    {
        Dictionary<string, int> ItemSaveDic = new Dictionary<string, int>();

        foreach (Slot slot in Inventory_Controller.g_ICinstance.g_Sslot)
        {
            if (slot != null && slot.g_Ihave_item != null)
            {
                ItemEntity.ItemStats_Save itemStats = new ItemEntity.ItemStats_Save();
                ItemSaveDic.Add(slot.g_Ihave_item.m_sItemName, slot.g_iitem_Number); // 아이템 이름 추가
                // 디버그 로그 출력
                Debug.Log("Added item: " + itemStats.m_sItemName + ", Count: " + itemStats.m_iItemCount);
            }
        }

        // itemStatsList의 요소 수를 확인하여 데이터가 있는지 확인
        Debug.Log("ItemStatsList Count: " + ItemSaveDic.Count);

        // 아이템 정보를 직렬화하여 파일로 저장
        string path = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        // JSON 문자열로 직렬화
        string json = JsonConvert.SerializeObject(ItemSaveDic, Formatting.Indented);

        // 파일에 쓰기
        File.WriteAllText(path, json);

        // 디버그 로그로 JSON 출력
        Debug.Log("Serialized JSON: " + json);
    }

    public static void LoadInventory()
    {
        // 파일에서 저장된 정보를 역직렬화하여 불러옴
        string path = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // 디버그 로그로 불러온 JSON 출력
            Debug.Log("Loaded JSON: " + json);

            Dictionary<string, int> tempDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            foreach(var item in tempDic)
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

}