using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[CreateAssetMenu(fileName = "NewBaseItem", menuName = "ItemEffect/Base")]
public class BaseItemEffect : SOItemBase
{
    public string m_sUseDialog;
    [Header("아이템 종류 회복, 공격력, 방어력, 스피드, 호감도, 효능, 신통방통")]
    public string g_sType;

    public int m_iEffectAmount;
    public int g_iStr; // 공격력
    public int g_iDef; // 방어력
    public int g_iSpd; // 스피드
    public int g_iLikeability; // 호감도

    public override void ExecuteItemEffect(UnitEntity allyUnit)
    {
        switch (g_sType)
        {
            case "회복":
                allyUnit.Heal(m_iEffectAmount);
                break;
            case "공격력":
                allyUnit.m_iPermanentAtkMod += g_iStr;
                break;
            case "방어력":
                allyUnit.m_iPermanentDefMod += g_iDef;
                break;
            case "스피드":
                allyUnit.m_iPermanentSpeedMod += g_iSpd;
                break;
            case "호감도":
                allyUnit.m_iIntimacy += 1;
                break;
            case "효능":
                allyUnit.g_UnitState.stateDur = 0;
                break;
            case "신통방통":
                allyUnit.m_iPermanentAtkMod += g_iStr;
                allyUnit.m_iPermanentDefMod += g_iDef;
                allyUnit.m_iPermanentSpeedMod += g_iSpd;
                break;
        }
    }
}
