using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehavior 
{
    //SO와 다중상속을 구현하기 위한 인터페이스입니다.
    //적/아군의 속성이나 방어력 값을 알아오기 위해 UnitEntity 공격자/ 방어자를 매개변수로 받습니다.
    void ExecuteAttack(UnitEntity Atker, UnitEntity Defender);
    string GetSkillName();
}
