using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DicCTR : MonoBehaviour,IPointerClickHandler
{
    private List<GameObject> m_DicElementsList;
    public GameObject m_DicElementPrefab;
    public GameObject g_ElementUI;
    public GameObject g_PortalEventButton;
    public GameObject g_PortalChangeButton;
    public ExplainAreaCTR g_ExplainArea;

    private void OnEnable()
    {
        //g_PortalEventButton = Instantiate(Resources.Load<GameObject>("Prefabs/PortalEventButton"),gameObject.transform);
        //g_PortalChangeButton = Instantiate(Resources.Load<GameObject>("Prefabs/PortalChange"), gameObject.transform);
        g_PortalEventButton.SetActive(false);
        g_PortalChangeButton.SetActive(false);
        InitList();
        InitUI();
        g_ExplainArea.Init("해태");
    }

    private void InitList()
    {
        m_DicElementsList = new List<GameObject>();

        foreach (var element in GameManager.Instance.m_DataManager.m_UnitDic)
        {
            if(element.Value.m_sUnitName.Equals("더미") || element.Value.m_sUnitName.Equals("일반 도깨비"))
                return;
            string unitName = element.Key;
            GameObject DicElementPrefab_temp = Instantiate(m_DicElementPrefab);
            DicElementPrefab_temp.GetComponent<DIcElementCTR>().Init(unitName);
            m_DicElementsList.Add(DicElementPrefab_temp);
        }
    }
   private void InitUI()
   {
        for (int i = 0; i < g_ElementUI.transform.childCount; i++)
            Destroy(g_ElementUI.transform.GetChild(i).gameObject);
        for (int i = 0; i < m_DicElementsList.Count; i++)
            m_DicElementsList[i].transform.SetParent(g_ElementUI.transform);
        
   }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (clickedObject.transform.GetComponent<DIcElementCTR>() != null)
        {
            if (GameManager.Instance.g_GameState == GameManager.GameState.PORTAL)
            {
                g_PortalEventButton.transform.position = clickedObject.transform.position;
                g_PortalEventButton.name = clickedObject.transform.GetComponent<DIcElementCTR>().m_UnitName;
                g_PortalEventButton.SetActive(true);
                g_PortalChangeButton.SetActive(false);
            }
            else
            {
                g_ExplainArea.Init(clickedObject.transform.GetComponent<DIcElementCTR>().m_UnitName);
            }
        }
    }

    public void PortalEvent_Info()
    {
        g_ExplainArea.Init(g_PortalEventButton.name);
        g_PortalEventButton.SetActive(false);
    }
    public void PortalEvent_Change()
    {
        g_PortalChangeButton.SetActive(true);
        g_PortalEventButton.SetActive(false);
        Vector3 pos = g_PortalEventButton.transform.position;
        pos.x += 200;
        g_PortalChangeButton.transform.position = pos;
        for (int i = 1; i < g_PortalChangeButton.transform.childCount; i++)
        {
            if(GameManager.Instance.m_UnitManager.g_PlayerUnits[i - 1] != null)
            {
                g_PortalChangeButton.transform.GetChild(i).transform.GetComponent<Button>().interactable = true;
                g_PortalChangeButton.transform.GetChild(i).transform.GetComponent<DIcElementCTR>()
                     .Init(GameManager.Instance.m_UnitManager.g_PlayerUnits[i - 1].transform.GetComponent<UnitEntity>().m_sUnitName);
            }
            else
            {
                Debug.Log(g_PortalChangeButton.transform.GetChild(i).name);
                g_PortalChangeButton.transform.GetChild(i).transform.GetComponent<Button>().interactable = true;
                g_PortalChangeButton.transform.GetChild(i).transform.GetComponent<DIcElementCTR>()
                    .Init("황룡");
            }
        }
    }
    public void ChangeUnit(int index)
    {
        if(GameManager.Instance.GetUnitSaveData(g_PortalEventButton.name).m_isCaptured)
        {
            for (int i = 0; i< GameManager.Instance.m_UnitManager.CheckUnitAmount();i++)
            {
                if (GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName == g_PortalEventButton.name)
                {
                    if (i != index && GameManager.Instance.m_UnitManager.g_PlayerUnits[index] != null)
                    {
                        GameManager.Instance.m_UnitManager.SetPlayerUnitEntityByName(GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>().m_sUnitName, i);
                        GameManager.Instance.m_UnitManager.SetPlayerUnitEntityByName(g_PortalEventButton.name, index);
                        g_PortalChangeButton.SetActive(false);
                        return;
                    }
                    else
                        return;
                }

            }
            if (GameManager.Instance.m_UnitManager.g_PlayerUnits[index] != null)
            {
                GameManager.Instance.SavePlayerUnit(GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>().m_sUnitName, GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>());
            }

            GameManager.Instance.m_UnitManager.SetPlayerUnitEntityByName(g_PortalEventButton.name, index);
            g_PortalChangeButton.SetActive(false);
            GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
        }
        else
        {
            Debug.Log(g_PortalEventButton.name);
        }
    }
}
