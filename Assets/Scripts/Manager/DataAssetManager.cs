using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAssetManager
{
    UnitTable m_AssetUnitTable;

    Dictionary<string, UnitTable.UnitStats> m_UnitDic;

    public void Init(UnitTable tableFromManager)
    {
        m_AssetUnitTable = tableFromManager;
        m_UnitDic = new Dictionary<string, UnitTable.UnitStats>();

        if (m_AssetUnitTable == null)
        {
            Debug.LogError("You Missed DataTable in GameManager");
            return;
        }

        foreach (var Unit in m_AssetUnitTable.m_Units)
        {
            m_UnitDic.Add(Unit.m_sUnitName, Unit);
        }

        Debug.Log("DataTable:" + m_AssetUnitTable.name + " Init Successful !,DataCount:" + m_UnitDic.Count);
    }

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

    public UnitTable.UnitStats GetUnitData(string className)
    {
        return m_UnitDic[className];
    }
}
