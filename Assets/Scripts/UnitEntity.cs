using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer { get; set; }

    public string m_sUnitName;
    public int m_iUnitHP;
    public int m_iUnitAtk;
    public int m_iUnitDef;
    public int m_iUnitSpeed;
    public int m_iCurrentHP;
    public int m_iUnitLevel;


    //공격 3가지
    IAttackBehavior m_AttackBehavior;
    IAttackBehavior m_AttackBehavior2;
    IAttackBehavior m_AttackBehavior3;

    //인덱스로 실행하는게 편할거같아서 만들었습니다
    IAttackBehavior[] m_AttackBehaviors;

    public void Awake()
    {
        if (m_sUnitName != null)
        {
            SetUnit(m_sUnitName);
            //Attack();
        }
    }
    //GameManager에 있는 UnitTable SO에 있는 정보를 기반으로 해당 게임오브젝트 초기화
    public void SetUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);

        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitHP = UnitData.m_iUnitHP;
        m_iUnitAtk = UnitData.m_iUnitAtk;
        m_iUnitDef = UnitData.m_iUnitDef;
        m_iUnitLevel = UnitData.m_iUnitLevel;
        m_iUnitSpeed = UnitData.m_iUnitSpeed;
        m_iCurrentHP = m_iUnitHP;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = UnitData.m_UnitSprite;
        m_AttackBehaviors = new IAttackBehavior[3];

        //인덱스로 실행하는게 편할거같아서 만들었습니다
        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);

        m_AttackBehavior = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehavior2 = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehavior3 = Instantiate(UnitData.m_AttackBehav_3);
    }


    public int Attack(UnitEntity Atker, UnitEntity Defender)
    {
        return m_AttackBehavior.ExecuteAttack(Atker,Defender);
    }

    //인덱스로 실행하는게 편할거같아서 만들었습니다
    public int AttackByIndex(UnitEntity Atker, UnitEntity Defender,int index)
    {
        return m_AttackBehaviors[index].ExecuteAttack(Atker, Defender);
    }


    // 체력을 회복하는 메서드
    public void Heal(int amount)
    {
        // 회복량을 현재 체력에 더함
        m_iCurrentHP += amount;

        // 만약 현재 체력이 최대 체력을 초과하면
        if (m_iCurrentHP > m_iUnitHP)
            // 현재 체력을 최대 체력으로 설정
            m_iCurrentHP = m_iUnitHP;
    }

}
