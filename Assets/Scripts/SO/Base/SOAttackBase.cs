using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOAttackBase : ScriptableObject, IAttackBehavior
{
    public int m_iSkillNo;
    public float m_fAttackMag;
    public string m_sAttackName;
    public string m_sSkillDescription;
    public int m_iUseAmount;
    public GameManager.Type SkillType;
    public enum SkillEffect { NULL, SLOW, CARELESSNESS, ULTIMATE, HALF, DEPRESSED, ONLYONCE }
    public UnitEntity.UnitState m_EffectState;
    public int m_iEffectDur;
    public int m_iEffectChance;
    public SkillEffect m_SkillEffect;
    public bool m_isPlayed;
    public int m_iAdditionalSpeed;
    public int EffectNum;

    public int m_IsDouble;
    public bool m_IsEffected;


    //공격을 구현할 인터페이스와 ScriptableObject를 상속받은 SOBase 파일입니다.
    public abstract void ExecuteAttack(UnitEntity Atker, UnitEntity Defender);
    public abstract string GetSkillName();
    public abstract int GetIsDouble();
    public abstract bool GetIsEffected();
}
