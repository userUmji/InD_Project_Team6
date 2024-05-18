using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class InventorySaveLoad : MonoBehaviour
{
    public static void SaveInventory()
    {
        List<ItemEntity.ItemStats_Save> itemStatsList = new List<ItemEntity.ItemStats_Save>();

        foreach (Slot slot in Inventory_Controller.g_ICinstance.g_Sslot)
        {
            if (slot != null && slot.g_Ihave_item != null)
            {
                ItemEntity.ItemStats_Save itemStats = new ItemEntity.ItemStats_Save();
                itemStats.m_sItemName = slot.g_Ihave_item.m_sItemName; // 아이템 이름 추가
                itemStats.m_iItemCount = slot.g_iitem_Number;
                itemStatsList.Add(itemStats);
                // 디버그 로그 출력
                Debug.Log("Added item: " + itemStats.m_sItemName + ", Count: " + itemStats.m_iItemCount);
            }
        }

        // itemStatsList의 요소 수를 확인하여 데이터가 있는지 확인
        Debug.Log("ItemStatsList Count: " + itemStatsList.Count);

        // 아이템 정보를 직렬화하여 파일로 저장
        string path = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        // JSON 문자열로 직렬화
        string json = JsonConvert.SerializeObject(itemStatsList, Formatting.Indented);

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

            ItemEntity.ItemStats_Save[] itemStatsArray = JsonConvert.DeserializeObject<ItemEntity.ItemStats_Save[]>(json);

            // g_ginventory 오브젝트의 자식 슬롯 수를 가져와서 배열의 길이 설정
            int slotCount = Inventory_Controller.g_ICinstance.g_ginventory.transform.childCount;
            Slot[] slots = new Slot[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                slots[i] = Inventory_Controller.g_ICinstance.g_ginventory.transform.GetChild(i).GetComponent<Slot>();
            }

            for (int i = 0; i < itemStatsArray.Length; i++)
            {
                ItemEntity.ItemStats_Save itemStats = itemStatsArray[i];
                Slot slot = slots[i % slotCount]; // 슬롯 순환을 위해 나머지 연산 사용

                // 아이템 갯수만큼 아이템을 생성하여 슬롯에 추가
                for (int j = 0; j < itemStats.m_iItemCount; j++)
                {
                    Debug.Log("Calling Input_Item method for slot " + i);
                    CreateItemAndAddToSlot(itemStats, slot); // 아이템 오브젝트 생성 후 슬롯에 추가
                }
            }
        }
    }

    private static void CreateItemAndAddToSlot(ItemEntity.ItemStats_Save itemStats, Slot slot)
    {
        // Resources 폴더에서 아이템 프리팹 로드
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + "ItemEntity");

        // 로드된 프리팹을 인스턴스화하여 아이템 오브젝트 생성
        GameObject itemObject = Instantiate(itemPrefab);
        itemObject.GetComponent<ItemEntity>().m_sItemName = itemStats.m_sItemName;
        itemObject.GetComponent<ItemEntity>().SetItemInfo();
        // 생성된 아이템 오브젝트를 비활성화
        itemObject.SetActive(false);

        // 인스턴스화된 아이템 오브젝트에서 ItemEntity 컴포넌트 가져오기
        ItemEntity itemEntity = itemObject.GetComponent<ItemEntity>();

        // ItemEntity 컴포넌트의 속성 설정
        itemEntity.m_sItemName = itemStats.m_sItemName;

        // 슬롯에 아이템 추가
        slot.Input_Item(itemEntity, 1); // 각 아이템은 1개씩 생성됨
    }

}