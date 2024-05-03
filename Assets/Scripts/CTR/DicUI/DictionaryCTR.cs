using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryCTR : MonoBehaviour
{
    public GameObject Element;
    public GameObject ViewPort;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        foreach(var Unit in GameManager.Instance.m_DataManager.m_UnitDic)
        {
            GameObject Element_Temp = Instantiate(Element,ViewPort.transform);
            Element_Temp.GetComponent<DIcElementCTR>().Init(Unit.Key);
        }
    }
}
