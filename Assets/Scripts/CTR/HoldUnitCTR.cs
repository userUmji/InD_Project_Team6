using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldUnitCTR : MonoBehaviour
{
    public GameObject UnitUIPrefab;
    public GameObject UnitUIArea;
    private void OnEnable()
    {
        UnitUIPrefab = Resources.Load<GameObject>("Prefabs/DicElement Variant");
        for (int i = 0; i < UnitUIArea.transform.childCount; i++)
            Destroy(UnitUIArea.transform.GetChild(i).gameObject);
        for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
        {
            GameObject Temp_UnitUI = Instantiate(UnitUIPrefab, UnitUIArea.transform);
            Temp_UnitUI.transform.GetComponent<DIcElementCTR>().Init(GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName);
        }
    }
}
