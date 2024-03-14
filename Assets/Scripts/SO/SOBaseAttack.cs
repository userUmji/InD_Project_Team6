using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Base")]
public class SOBaseAttack : SOAttackBase
{
    float CriticalChance = 10.0f;
    int AttackMag = 1;
    public override int ExecuteAttack(int baseAttackDamage)
    {
        int finalAttackDamage = baseAttackDamage * AttackMag;

        Debug.Log("BaseAttack!" + finalAttackDamage);
        return finalAttackDamage;
    }
}
