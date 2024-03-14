using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer { get; set; }

    public string m_sUnitName;
    public int m_iHealthPoint;
    public float m_fAttackDamage;
    public int m_iAttackDamage;
    public int m_iCurrentHP;
    public int m_iUnitLevel;

    IAttackBehavior m_AttackBehavior;

    public void Awake()
    {
        if (m_sUnitName != null)
        {
            SetUnit(m_sUnitName);
            //Attack();
        }
    }
    //UnitTable SO에 있는 정보를 기반으로 해당 게임오브젝트 초기화
    public void SetUnit(string className)
    {
        
        var UnitData = GameManager.Instance.GetUnitData(className);

        m_sUnitName = UnitData.m_sUnitName;


        gameObject.name += "-" + m_sUnitName;

        m_iHealthPoint = UnitData.m_iHealthPoint;
        m_iAttackDamage = UnitData.m_iAttackDamage;
        m_iUnitLevel = UnitData.m_iUnitLevel;
        m_iCurrentHP = m_iHealthPoint;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = UnitData.m_UnitSprite;

        m_AttackBehavior = Instantiate(UnitData.m_AttackBehav_1);
    }


    public int Attack()
    {
        return m_AttackBehavior.ExecuteAttack(m_iAttackDamage);
    }

    // 대미지를 입는 메서드
    public bool TakeDamage(int dmg)
    {
        // 현재 체력에서 대미지를 감소시킴
        m_iCurrentHP -= dmg;

        // 만약 현재 체력이 0 이하라면
        if (m_iCurrentHP <= 0)
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
        m_iCurrentHP += amount;

        // 만약 현재 체력이 최대 체력을 초과하면
        if (m_iCurrentHP > m_iHealthPoint)
            // 현재 체력을 최대 체력으로 설정
            m_iCurrentHP = m_iHealthPoint;
    }

}
