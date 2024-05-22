using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Base")]
public class SOBaseAttack : SOAttackBase
{
    //m_fAttackMag -> 공격 배율
    //SkillType -> 스킬의 타입
    
    public override void ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        m_IsEffected = false;  
        // 최종 데미지 -> ((공격력 * 공격 배율) - 방어력)*속성 배율
        int AttackDamage = (int)((Atker.m_iUnitAtk + Atker.m_iTempAtkMod + Atker.m_iPermanentAtkMod)  * m_fAttackMag);
        int finalAttackDamage = AttackDamage - (Defender.m_iUnitDef + Defender.m_iPermanentDefMod + Defender.m_iTempDefMod);

        m_IsDouble = GameManager.Instance.CompareType(SkillType, Defender.UnitType);

        if (m_IsDouble == 1)
            finalAttackDamage = (int)(finalAttackDamage * 1.2);
        else if (m_IsDouble == 2)
            finalAttackDamage = (int)(finalAttackDamage * 0.8);

        Defender.m_iCurrentHP -= finalAttackDamage;

    }

    public override string GetSkillName()
    {
        return m_sAttackName;
    }

    public override int GetIsDouble()
    {
        return m_IsDouble;
    }

    public override bool GetIsEffected()
    {
        return false;
    }
}
