using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{
    public GameObject[] g_PlayerUnits = new GameObject[3];
    private GameObject UnitPrefab = Resources.Load<GameObject>("Prefabs/UnitEntity");

    public GameObject GetUnitEntity(int index)
    {
        return g_PlayerUnits[index];
    }
    public void SetPlayerUnitEntityByName(string unitName, int index)
    {
        GameObject UnitEntity_temp = GameObject.Instantiate(UnitPrefab);
        UnitEntity_temp.transform.GetComponent<UnitEntity>().SetUnit(unitName);
        g_PlayerUnits[index] = UnitEntity_temp;
    }
    public GameObject SetUnitEntityByName(string unitName)
    {
        GameObject UnitEntity_temp = GameObject.Instantiate(UnitPrefab);
        UnitEntity_temp.transform.GetComponent<UnitEntity>().SetUnit(unitName);
        return UnitEntity_temp;
    }
    public void SetUnitEntityByGO(GameObject unitGO, int index)
    {
        g_PlayerUnits[index] = unitGO;
    }

}
