using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Base")]
public class SOBaseAttack : SOAttackBase
{
    //유닛들의 공통적이거나 개별적인 공격입니다.
    //SO를 사용해 SOInstance를 생성하고 사용합니다.

    //이 부분에서 해당 공격의 구현 방식을 설정할 수 있습니다.
    float CriticalChance = 10.0f;
    float AttackMag = 1.2f;
    
    public override int ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        
        int AttackDamage =(int)(Atker.m_iUnitAtk * AttackMag);
        int finalAttackDamage = AttackDamage - Defender.m_iUnitDef;
        
        Debug.Log("BaseAttack!" + finalAttackDamage);

        Defender.m_iCurrentHP -= finalAttackDamage;
        return finalAttackDamage;
    }
}
