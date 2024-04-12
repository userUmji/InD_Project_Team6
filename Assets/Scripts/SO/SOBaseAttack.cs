using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Base")]
public class SOBaseAttack : SOAttackBase
{
    //���ֵ��� �������̰ų� �������� �����Դϴ�.
    //SO�� ����� SOInstance�� �����ϰ� ����մϴ�.
    //m_fAttackMag -> ��ų ����
    //SkillType -> ��ų Ÿ��
    //�� �κп��� �ش� ������ ���� ����� ������ �� �ֽ��ϴ�.


    public override void ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        // ���� -> ((���ݷ�*��ų����) - ������)*�Ӽ�����
        int AttackDamage = (int)((Atker.m_iUnitAtk + Atker.m_iTempAtkMod + Atker.m_iPermanentAtkMod)  * m_fAttackMag);
        int finalAttackDamage = AttackDamage - (Defender.m_iUnitDef + Defender.m_iPermanentDefMod + Defender.m_iTempDefMod);

        int isDouble = GameManager.Instance.CompareType(SkillType, Defender.UnitType);

        if (isDouble == 1)
            finalAttackDamage = (int)(finalAttackDamage * 1.2);
        else if (isDouble == 2)
            finalAttackDamage = (int)(finalAttackDamage * 0.8);

        Defender.m_iCurrentHP -= finalAttackDamage;

        //Debug.Log(isDouble);
        //Debug.Log(finalAttackDamage);
    }

    public override string GetSkillName()
    {
        return m_sAttackName;
    }
}
