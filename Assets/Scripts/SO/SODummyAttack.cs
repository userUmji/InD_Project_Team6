using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Dummy")]
public class SODummyAttack : SOAttackBase
{
    //유닛들의 공통적이거나 개별적인 공격입니다.
    //SO를 사용해 SOInstance를 생성하고 사용합니다.
    //m_fAttackMag -> 스킬 배율
    //SkillType -> 스킬 타입
    //이 부분에서 해당 공격의 구현 방식을 설정할 수 있습니다.


    public override void ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        // 계산식 -> ((공격력*스킬배율) - 적방어력)*속성배율
        int AttackDamage = (int)((Atker.m_iUnitAtk + Atker.m_iTempAtkMod + Atker.m_iPermanentAtkMod) * m_fAttackMag);
        int finalAttackDamage = AttackDamage - (Defender.m_iUnitDef + Defender.m_iPermanentDefMod + Defender.m_iTempDefMod);

        int isDouble = GameManager.Instance.CompareType(SkillType, Defender.UnitType);

        if (isDouble == 1)
            finalAttackDamage = (int)(finalAttackDamage * 1.2);
        else if (isDouble == 2)
            finalAttackDamage = (int)(finalAttackDamage * 0.8);

        Defender.TakeDamage(finalAttackDamage);
       // Defender.g_UnitState = UnitEntity.UnitState.BERSERK;
        //Debug.Log(isDouble);
        //Debug.Log(finalAttackDamage);
    }

    public override int GetIsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override bool GetIsEffected()
    {
        throw new System.NotImplementedException();
    }

    public override string GetSkillName()
    {
        return m_sAttackName;
    }
}
