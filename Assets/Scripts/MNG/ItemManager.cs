using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ItemManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static ItemManager Instance { get; private set; }

    // ������ ����� ������ ����Ʈ
    public List<ItemEntity> itemList;

    // JSON ���� ���
    private string saveFilePath;

    void Awake()
    {
        // �ν��Ͻ� ����
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // �ٸ� �ν��Ͻ��� �̹� �����ϸ� �ڽ��� �ı�

        // JSON ���� ��� ����
        saveFilePath = Application.persistentDataPath + "/itemList.json";
    }

    void Start()
    {
        // ��� �ҷ�����
        LoadItemList();

        // ��Ͽ� �ִ� �����۸� Ȱ��ȭ
        EnableItemList();

        // ������ �±׸� ���� ��� �������� ��Ȱ��ȭ
        DisableItemsWithTag("Item");
    }

    // ��Ͽ� �ִ� �����۸� Ȱ��ȭ
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

    // ������ �±׸� ���� ��� �������� ��Ȱ��ȭ (����Ʈ�� �ִ� �������� ����)
    private void DisableItemsWithTag(string tag)
    {
        GameObject[] itemsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in itemsWithTag)
        {
            // ����Ʈ�� �ִ� ���������� Ȯ��
            ItemEntity itemEntity = item.GetComponent<ItemEntity>();
            if (itemEntity != null && itemList.Contains(itemEntity))
            {
                continue; // ����Ʈ�� �ִ� �������̸� �Ѿ
            }

            item.SetActive(false); // ����Ʈ�� ���� �������� ��Ȱ��ȭ
        }
    }

    // ������ ����
    public void RemoveItem(ItemEntity item)
    {
        if (itemList.Contains(item))
        {
            itemList.Remove(item); // �������� ����Ʈ���� ����
            Debug.Log(item.name + " removed from the item list.");
        }
        else
        {
            Debug.LogWarning(item.name + " is not in the item list.");
        }
    }

    // ������ ��� ����
    public void SaveItemList()
    {
        string json = JsonUtility.ToJson(itemList);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Item list saved to " + saveFilePath);
    }

    // ������ ��� �ҷ�����
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