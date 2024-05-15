using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateItem : SOItemBase
{
    public int m_iEffectAmount;
    public string m_sEffectState;
    public string m_sUseDialog;

    public override void ExecuteItemEffect(UnitEntity allyUnit)
    {
        if (m_sEffectState == "ATK")
            allyUnit.m_iPermanentAtkMod += m_iEffectAmount;
        else if (m_sEffectState == "DEF")
            allyUnit.m_iPermanentDefMod += m_iEffectAmount;
    }
}