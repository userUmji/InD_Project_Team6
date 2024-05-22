using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ItemManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ItemManager Instance { get; private set; }

    // 아이템 목록을 저장할 리스트
    public List<ItemEntity> itemList;

    // JSON 파일 경로
    private string saveFilePath;

    void Awake()
    {
        // 인스턴스 설정
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // 다른 인스턴스가 이미 존재하면 자신을 파괴

        // JSON 파일 경로 설정
        saveFilePath = Application.persistentDataPath + "/itemList.json";
    }

    void Start()
    {
        // 목록 불러오기
        LoadItemList();

        // 목록에 있는 아이템만 활성화
        EnableItemList();

        // 아이템 태그를 가진 모든 아이템을 비활성화
        DisableItemsWithTag("Item");
    }

    // 목록에 있는 아이템만 활성화
    private void EnableItemList()
    {
        foreach (ItemEntity item in itemList)
        {
            if (item != null && item.gameObject != null)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    // 아이템 태그를 가진 모든 아이템을 비활성화 (리스트에 있는 아이템은 제외)
    private void DisableItemsWithTag(string tag)
    {
        GameObject[] itemsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in itemsWithTag)
        {
            // 리스트에 있는 아이템인지 확인
            ItemEntity itemEntity = item.GetComponent<ItemEntity>();
            if (itemEntity != null && itemList.Contains(itemEntity))
            {
                continue; // 리스트에 있는 아이템이면 넘어감
            }

            item.SetActive(false); // 리스트에 없는 아이템은 비활성화
        }
    }

    // 아이템 제거
    public void RemoveItem(ItemEntity item)
    {
        if (itemList.Contains(item))
        {
            itemList.Remove(item); // 아이템을 리스트에서 제거
            Debug.Log(item.name + " removed from the item list.");
        }
        else
        {
            Debug.LogWarning(item.name + " is not in the item list.");
        }
    }

    // 아이템 목록 저장
    public void SaveItemList()
    {
        string json = JsonUtility.ToJson(itemList);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Item list saved to " + saveFilePath);
    }

    // 아이템 목록 불러오기
    public void LoadItemList()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            itemList = JsonUtility.FromJson<List<ItemEntity>>(json);
            Debug.Log("Item list loaded from " + saveFilePath);
        }
        else
        {
            itemList = new List<ItemEntity>();
            Debug.Log("No item list found at " + saveFilePath + ". Creating new list.");
        }
    }
}