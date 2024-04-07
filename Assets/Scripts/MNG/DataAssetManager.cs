using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DataAssetManager
{
    //Asset에 있는 SO 유닛 테이블 인스턴스
    UnitTable m_AssetUnitTable;
    ItemTable m_AssetItemTable;
    //유닛의 이름을 Key값으로 묶어 저장하는 Dictionary
    Dictionary<string, UnitTable.UnitStats> m_UnitDic;
    Dictionary<string, UnitTable.UnitStats_Save> m_UnitSaveDic;
    Dictionary<string, ItemTable.ItemStats> m_ItemDic;

    //Init 기능
    public void Init(UnitTable tableFromManager_Unit,ItemTable tableFromManager_Item)
    {
        m_AssetUnitTable = tableFromManager_Unit;
        m_AssetItemTable = tableFromManager_Item;
        m_UnitDic = new Dictionary<string, UnitTable.UnitStats>();
        m_ItemDic = new Dictionary<string, ItemTable.ItemStats>();
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

        Debug.Log("DataTable:" + m_AssetUnitTable.name + " Init Successful !,DataCount:" + m_UnitDic.Count);
    }
    //데이터 안전하게 불러오기
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
    //데이터 불러오기
    public UnitTable.UnitStats GetUnitData(string className)
    {
        return m_UnitDic[className];
    }
    public ItemTable.ItemStats GetItemData(string className)
    {
        return m_ItemDic[className];
    }

    public void SaveFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        string SaveData = JsonConvert.SerializeObject(m_UnitSaveDic);
        File.WriteAllText(path, SaveData);
        Debug.Log("Save Complete");
    }
    public void LoadFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path))
        {
            System.IO.File.Delete(path);
            SaveFunc();
            string JsonDataTemp = File.ReadAllText(path);
            m_UnitSaveDic = JsonConvert.DeserializeObject<Dictionary<string, UnitTable.UnitStats_Save>>(JsonDataTemp);
            Debug.Log(path);

        }
        else
        {
            SaveFunc();
            LoadFunc();
            Debug.Log("Savefile missed. created new Savefile");
        }
    }
}
