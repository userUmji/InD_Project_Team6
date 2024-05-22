using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[CreateAssetMenu(fileName = "NewBaseItem", menuName = "ItemEffect/Base")]
public class BaseItemEffect : SOItemBase
{
    public string m_sUseDialog;
    [Header("������ ���� ȸ��, ���ݷ�, ����, ���ǵ�, ȣ����, ȿ��, �������")]
    public string g_sType;

    public int m_iEffectAmount;
    public int g_iStr; // ���ݷ�
    public int g_iDef; // ����
    public int g_iSpd; // ���ǵ�
    public int g_iLikeability; // ȣ����

    public override void ExecuteItemEffect(UnitEntity allyUnit)
    {
        switch (g_sType)
        {
            case "ȸ��":
                allyUnit.Heal(m_iEffectAmount);
                break;
            case "���ݷ�":
                allyUnit.m_iPermanentAtkMod += g_iStr;
                break;
            case "����":
                allyUnit.m_iPermanentDefMod += g_iDef;
                break;
            case "���ǵ�":
                allyUnit.m_iPermanentSpeedMod += g_iSpd;
                break;
            case "ȣ����":
                allyUnit.m_iIntimacy += 1;
                break;
            case "ȿ��":
                allyUnit.g_UnitState.stateDur = 0;
                break;
            case "�������":
                allyUnit.m_iPermanentAtkMod += g_iStr;
                allyUnit.m_iPermanentDefMod += g_iDef;
                allyUnit.m_iPermanentSpeedMod += g_iSpd;
                break;
        }
    }
}
