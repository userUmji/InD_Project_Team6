using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/State")]
public class SOStateAttack : SOAttackBase
{
    //m_fAttackMag -> 공격 배율
    //SkillType -> 스킬의 타입

    public override void ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        m_IsEffected = false;
        // 최종 데미지 -> ((공격력 * 공격 배율) - 방어력)*속성 배율
        int AttackDamage = (int)((Atker.m_iUnitAtk + Atker.m_iTempAtkMod + Atker.m_iPermanentAtkMod) * m_fAttackMag);
        if (m_SkillEffect == SkillEffect.ULTIMATE)
        {
            m_IsEffected = true;
            float damage = AttackDamage * 1.2f;
            AttackDamage = (int)damage;
        }
            

        int finalAttackDamage = AttackDamage - (Defender.m_iUnitDef + Defender.m_iPermanentDefMod + Defender.m_iTempDefMod);

        m_IsDouble = GameManager.Instance.CompareType(SkillType, Defender.UnitType);

        if (m_IsDouble == 1)
            finalAttackDamage = (int)(finalAttackDamage * 1.2f);
        else if (m_IsDouble == 2)
            finalAttackDamage = (int)(finalAttackDamage * 0.8f);

        Defender.TakeDamage(finalAttackDamage);

        ApplyEffect(Atker, Defender, finalAttackDamage);

        m_isPlayed = true;
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
    public void ApplyEffect_UnitState(UnitEntity entity, SkillEffect effect)
    {
        m_IsEffected = true;
        if (effect == SkillEffect.SLOW)
            entity.m_iTempSpeedMod -= 3;
        else if (effect == SkillEffect.CARELESSNESS)
            entity.m_iTempDefMod -= 3;
        else if (effect == SkillEffect.DEPRESSED)
        {
            entity.m_iTempDefMod -= 3;
            entity.m_iTempAtkMod -= 3;
            entity.m_iTempSpeedMod -= 3;
        }
    }
    private void ApplyEffect(UnitEntity Atker, UnitEntity Defender , int finalAttackDamage)
    {
        int randomEffectChance = Random.Range(0, 101);
        if (m_EffectState != UnitEntity.UnitState.NULL)
        {
            if (m_iEffectChance < randomEffectChance)
            {
                m_IsEffected = true;
                Defender.g_UnitState.state = m_EffectState;
                Defender.g_UnitState.stateDur = m_iEffectDur;
                if (m_EffectState == UnitEntity.UnitState.FIRE || m_EffectState == UnitEntity.UnitState.ICE)
                    Defender.g_UnitState.stateDamage = finalAttackDamage / 10;
            }
            else
                m_IsEffected = false;
        }
        if (m_SkillEffect != SkillEffect.NULL || m_SkillEffect != SkillEffect.HALF || m_SkillEffect != SkillEffect.ONLYONCE || m_SkillEffect != SkillEffect.ULTIMATE)
        {
            if (m_iEffectChance < randomEffectChance)
            {
                ApplyEffect_UnitState(Defender, m_SkillEffect);
            }
        }
        if (m_SkillEffect == SkillEffect.HALF)
        {
            m_IsEffected = true;
            float hp = Defender.m_iCurrentHP / 30.0f;
            Defender.m_iCurrentHP = (int)hp;
        }
        if (m_SkillEffect == SkillEffect.NULL && m_EffectState == UnitEntity.UnitState.NULL)
            m_IsEffected = false;
    }
}
