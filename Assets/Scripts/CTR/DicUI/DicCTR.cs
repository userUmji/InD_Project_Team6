using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DicCTR : MonoBehaviour,IPointerClickHandler
{
    private List<GameObject> m_DicElementsList;
    public GameObject m_DicElementPrefab;
    public GameObject g_ElementUI;
    public ExplainAreaCTR g_ExplainArea;

    private void OnEnable()
    {
        InitList();
        InitUI();
        g_ExplainArea.Init("해태");
    }

    private void InitList()
    {
        m_DicElementsList = new List<GameObject>();
        foreach (var element in GameManager.Instance.m_DataManager.m_UnitDic)
        {
            if(element.Value.m_sUnitName.Equals("더미"))
                return;
            string unitName = element.Key;
            GameObject DicElementPrefab_temp = Instantiate(m_DicElementPrefab);
            DicElementPrefab_temp.GetComponent<DIcElementCTR>().Init(unitName);
            m_DicElementsList.Add(DicElementPrefab_temp);
        }
    }
   private void InitUI()
   {
        for(int i = 0; i<m_DicElementsList.Count;i++)
        {
            m_DicElementsList[i].transform.SetParent(g_ElementUI.transform);
        }
   }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject.transform.GetComponent<DIcElementCTR>() != null)
        {
            g_ExplainArea.Init(clickedObject.transform.GetComponent<DIcElementCTR>().UnitName.text);
        }
    }
}
