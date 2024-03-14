using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // 유닛의 이름
    public string unitName;

    // 유닛의 레벨
    public int unitLevel;

    // 유닛이 입히는 데미지
    public int damage;

    // 유닛의 최대 체력
    public int maxHP;

    // 유닛의 현재 체력
    public int currentHP;

    // 대미지를 입는 메서드
    public bool TakeDamage(int dmg)
    {
        // 현재 체력에서 대미지를 감소시킴
        currentHP -= dmg;

        // 만약 현재 체력이 0 이하라면
        if (currentHP <= 0)
            // 유닛이 죽었음을 나타내는 true 반환
            return true;
        else
            // 아니면 살아있음을 나타내는 false 반환
            return false;
    }

    // 체력을 회복하는 메서드
    public void Heal(int amount)
    {
        // 회복량을 현재 체력에 더함
        currentHP += amount;

        // 만약 현재 체력이 최대 체력을 초과하면
        if (currentHP > maxHP)
            // 현재 체력을 최대 체력으로 설정
            currentHP = maxHP;
    }
}
