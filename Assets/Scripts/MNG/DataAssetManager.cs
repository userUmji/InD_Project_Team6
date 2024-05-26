using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public class DataAssetManager
{
    //Asset 테이블 정보
    UnitTable m_AssetUnitTable;
    ItemTable m_AssetItemTable;
    //아이템/유닛의 이름이 key값, 정보가 Value값
    public Dictionary<string, UnitTable.UnitStats> m_UnitDic;
    Dictionary<string, ItemTable.ItemStats> m_ItemDic;
    Dictionary<string, UnitTable.UnitStats_Save> m_UnitSaveDic;
    public List<SOAttackBase> g_Skills;

    //Init
    public void Init(UnitTable tableFromManager_Unit,ItemTable tableFromManager_Item)
    {
        m_AssetUnitTable = tableFromManager_Unit;
        m_AssetItemTable = tableFromManager_Item;
        m_UnitDic = new Dictionary<string, UnitTable.UnitStats>();
        m_ItemDic = new Dictionary<string, ItemTable.ItemStats>();
        m_UnitSaveDic = new Dictionary<string, UnitTable.UnitStats_Save>();
        string path = Application.persistentDataPath + "Save.json";
        if (!File.Exists(path))
        {
            InitSaveData();
            SaveFunc_ALL();
        }

        LoadFunc();
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
    //데이터 안전놀이터
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
    public UnitTable.UnitStats_Save GetUnitSaveData(string className)
    {
        return m_UnitSaveDic[className];
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
            SaveData.m_iUnitLevel = 5;
            m_UnitSaveDic.Add(Unit.m_sUnitName, SaveData);
        }
    }
    public void SaveByUnit(string className, UnitEntity saveData)
    {
        UnitTable.UnitStats_Save SaveData_Temp = new UnitTable.UnitStats_Save();
        SaveData_Temp.m_isCaptured = true;
        SaveData_Temp.m_iIntimacy = saveData.m_iIntimacy;
        SaveData_Temp.m_iPermanentAtkMod = saveData.m_iPermanentAtkMod;
        SaveData_Temp.m_iPermanentDefMod = saveData.m_iPermanentDefMod;
        SaveData_Temp.m_iPermanentSpeedMod = saveData.m_iPermanentSpeedMod;
        SaveData_Temp.m_iUnitEXP = saveData.m_iUnitEXP;
        SaveData_Temp.m_iUnitLevel = saveData.m_iUnitLevel;
        SaveData_Temp.m_AttackBehav_1 = saveData.m_AttackBehaviors[0].m_iSkillNo;
        SaveData_Temp.m_AttackBehav_2 = saveData.m_AttackBehaviors[1].m_iSkillNo;
        SaveData_Temp.m_AttackBehav_3 = saveData.m_AttackBehaviors[2].m_iSkillNo;

        m_UnitSaveDic[className] = SaveData_Temp;
    }

    public void SaveFunc_ALL()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path))
            System.IO.File.Delete(path);
        string SaveData = JsonConvert.SerializeObject(m_UnitSaveDic,Formatting.Indented);
        File.WriteAllText(path, SaveData);
        Debug.Log("Save Complete " + path);
    }
    public void LoadFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path))
        {
            string JsonDataTemp = File.ReadAllText(path);
            m_UnitSaveDic = JsonConvert.DeserializeObject<Dictionary<string, UnitTable.UnitStats_Save>>(JsonDataTemp);
        }
        else
        {
            InitSaveData();
            SaveFunc_ALL();
            LoadFunc();
            Debug.Log("Savefile missed. created new Savefile");
        }
    }
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
        if (txtFileTemp.Trim().Length==0)
        {
            txtFileTemp = "해태";
        }
        splitNames = txtFileTemp.Split(',');
        Debug.Log(splitNames.Length);
        for (int i = 0;i < splitNames.Length;i++)
        {
            if (splitNames[i].Trim().Length != 0)
            {
                Debug.Log(splitNames[i]);
                GameManager.Instance.m_UnitManager.SetPlayerUnitEntityByName(splitNames[i], i);
            }

        }
    }

}
