using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public class DataAssetManager
{
    //Asset�� �ִ� SO ���� ���̺� �ν��Ͻ�
    UnitTable m_AssetUnitTable;
    ItemTable m_AssetItemTable;
    //������ �̸��� Key������ ���� �����ϴ� Dictionary
    Dictionary<string, UnitTable.UnitStats> m_UnitDic;
    Dictionary<string, ItemTable.ItemStats> m_ItemDic;
    Dictionary<string, UnitTable.UnitStats_Save> m_UnitSaveDic;

    //Init ���
    public void Init(UnitTable tableFromManager_Unit,ItemTable tableFromManager_Item)
    {
        m_AssetUnitTable = tableFromManager_Unit;
        m_AssetItemTable = tableFromManager_Item;
       m_UnitDic = new Dictionary<string, UnitTable.UnitStats>();
        m_ItemDic = new Dictionary<string, ItemTable.ItemStats>();
        m_UnitSaveDic = new Dictionary<string, UnitTable.UnitStats_Save>();
        if (m_AssetUnitTable == null)
        {
            Debug.LogError("You Missed DataTable in GameManager");
            return;
        }

        if (m_AssetItemTable == null)
        {
            Debug.LogError("You Missed DataTable in GameManager");
            return;
        }

        foreach (var Unit in m_AssetUnitTable.m_Units)
        {
            m_UnitDic.Add(Unit.m_sUnitName, Unit);
        }

        foreach (var Item in m_AssetItemTable.m_Items)
        {
            m_ItemDic.Add(Item.m_sItemName, Item);
        }

        InitSaveData();
        Debug.Log("DataTable:" + m_AssetUnitTable.name + " Init Successful !,DataCount:" + m_UnitDic.Count);
    }
    //������ �����ϰ� �ҷ�����
    public bool GetUnitDataSafe(string className, out UnitTable.UnitStats foundUnitStat)
    {
        if (!m_UnitDic.ContainsKey(className))
        {
            Debug.LogError("There Is No :" + className + "In " + m_AssetUnitTable.name + " Table");
            foundUnitStat = new UnitTable.UnitStats();
            return false;
        }
        foundUnitStat = m_UnitDic[className];
        return true;
    }
    //������ �ҷ�����
    public UnitTable.UnitStats GetUnitData(string className)
    {
        return m_UnitDic[className];
    }
    public ItemTable.ItemStats GetItemData(string className)
    {
        return m_ItemDic[className];
    }
    public void InitSaveData()
    {
        m_UnitSaveDic = new Dictionary<string, UnitTable.UnitStats_Save>();
        foreach (var Unit in m_AssetUnitTable.m_Units)
        {
            UnitTable.UnitStats_Save SaveData = new UnitTable.UnitStats_Save();
            SaveData.m_iIntimacy = 0;
            SaveData.m_iPermanentAtkMod = 0;
            SaveData.m_iPermanentDefMod = 0;
            SaveData.m_iPermanentSpeedMod = 0;
            SaveData.m_iUnitEXP = 0;
            SaveData.m_iUnitLevel = 10;

            m_UnitSaveDic.Add(Unit.m_sUnitName, SaveData);
            
        }
    }
    public void SaveFunc_ALL()
    {
        string path = Application.persistentDataPath + "Save.json";
        string SaveData = JsonConvert.SerializeObject(m_UnitSaveDic);
        File.WriteAllText(path, SaveData);
        Debug.Log("Save Complete " + path);
    }
    public void LoadFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path))
        {
            System.IO.File.Delete(path);
            SaveFunc_ALL();
            string JsonDataTemp = File.ReadAllText(path);
            m_UnitSaveDic = JsonConvert.DeserializeObject<Dictionary<string, UnitTable.UnitStats_Save>>(JsonDataTemp);
            Debug.Log(path);

        }
        else
        {
            SaveFunc_ALL();
            LoadFunc();
            Debug.Log("Savefile missed. created new Savefile");
        }
    }
}
