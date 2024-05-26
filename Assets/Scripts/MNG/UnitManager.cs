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

        GameObject UnitEntity_temp = GameObject.Instantiate(UnitPrefab,GameObject.Find("PlayerUnits").transform);
        UnitEntity_temp.transform.GetComponent<UnitEntity>().SetPlayerUnit(unitName);
        GameObject.Destroy(g_PlayerUnits[index]);
        g_PlayerUnits[index] = UnitEntity_temp;
    }
    public GameObject SetUnitEntityByName(string unitName, int lvl)
    {
        GameObject UnitEntity_temp = GameObject.Instantiate(UnitPrefab);
        UnitEntity_temp.transform.GetComponent<UnitEntity>().SetUnit(unitName,lvl);
        return UnitEntity_temp;
    }
    public void SetUnitEntityByGO(GameObject unitGO, int index)
    {
        g_PlayerUnits[index] = unitGO;
    }
    public int CheckUnitAmount()
    {
        int amount = 0;
        for (int i = 0; i< g_PlayerUnits.Length; i++)
        {
            if (g_PlayerUnits[i] != null)
                amount += 1;
        }
        return amount;
    }

}
