using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOAttackBase : ScriptableObject , IAttackBehavior
{
    public GameManager.Type SkillType;
    public float m_fAttackMag;
    public string m_sAttackName;
    //공격을 구현할 인터페이스와 ScriptableObject를 상속받은 SOBase 파일입니다.
    public abstract void ExecuteAttack(UnitEntity Atker, UnitEntity Defender);
    public abstract string GetSkillName();
}
