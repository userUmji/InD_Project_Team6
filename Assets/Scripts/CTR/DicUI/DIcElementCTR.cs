using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DIcElementCTR : MonoBehaviour
{
    public Image UnitImage;
    public TextMeshProUGUI UnitName;
    public TextMeshProUGUI UnitLevel;
    public Slider UnitHP;
    public Slider UnitIntimacy;
    public UnitTable.UnitStats Unit;
    public UnitTable.UnitStats_Save SaveUnit;


    public void Init(string name)
    {
        var Unit = GameManager.Instance.GetUnitData(name);
        var SaveUnit = GameManager.Instance.GetUnitSaveData(name);
        UnitImage.sprite = Unit.m_UnitSprite;
        UnitName.text = name;
        UnitLevel.text = SaveUnit.m_iUnitLevel + "Lvl";
        UnitHP.maxValue = Unit.m_iUnitHP + (SaveUnit.m_iUnitLevel * 3);
        UnitHP.value = UnitHP.maxValue;
        UnitIntimacy.maxValue = 10;
        UnitIntimacy.value = SaveUnit.m_iIntimacy;
    }
    public string OnMouseEvent()
    {
        return UnitName.text;
    }

}
