using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseItem", menuName = "ItemEffect/Base")]
public class BaseItemEffect : SOItemBase
{
    public int m_iEffectAmount;
    public string m_sUseDialog;
   
    public override void ExecuteItemEffect(UnitEntity allyUnit)
    {
        allyUnit.Heal(m_iEffectAmount);
    }
}
