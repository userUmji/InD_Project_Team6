using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DIcElementCTR : MonoBehaviour
{
    public string m_UnitName;
    public Image UnitImage;
    public TextMeshProUGUI UnitName;
    public TextMeshProUGUI UnitLevel;
    public Slider UnitHP;
    public Slider UnitIntimacy;
    public UnitTable.UnitStats Unit;
    public UnitTable.UnitStats_Save SaveUnit;


    public void Init(string name)
    {
        m_UnitName = name;
        var Unit = GameManager.Instance.GetUnitData(m_UnitName);
        var SaveUnit = GameManager.Instance.GetUnitSaveData(m_UnitName);
        if(SaveUnit.m_isCaptured)
        {
            UnitImage.sprite = Unit.m_UnitSprite;
            UnitName.text = m_UnitName;
            UnitLevel.text = SaveUnit.m_iUnitLevel + "Lvl";
            UnitHP.maxValue = Unit.m_iUnitHP + ((SaveUnit.m_iUnitLevel - 5) * 2 * SaveUnit.m_iUnitLevel);
            for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
            {
                if (m_UnitName == GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName)
                    UnitHP.value = GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_iCurrentHP;
                else
                    UnitHP.value = UnitHP.maxValue;
            }
            UnitIntimacy.maxValue = 10;
            UnitIntimacy.value = SaveUnit.m_iIntimacy;
        }
        else
        {
            UnitImage.sprite = Resources.Load<Sprite>("Images/Unit/UnknownUnit");
            UnitName.text = "???";
            UnitLevel.text = "???";
            UnitHP.maxValue = 10;
            UnitHP.value = 10;
            UnitIntimacy.maxValue = 10;
            UnitIntimacy.value = 0;

        }

    }
    public string OnMouseEvent()
    {
        return UnitName.text;
    }

}
