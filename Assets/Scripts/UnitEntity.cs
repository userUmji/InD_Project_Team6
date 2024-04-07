using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{

    #region 유닛 정보
    public string m_sUnitName;
    public int m_iUnitHP;
    public int m_iUnitAtk;
    public int m_iUnitDef;
    public int m_iUnitSpeed;
    public int m_iCurrentHP;
    public int m_iUnitAtkMod_Temp;
    public int m_iUnitDefMod_Temp;
    public int[] m_iSkillAmounts;
    public Sprite m_spriteUnitImage;
    public GameManager.Type UnitType;
    #endregion
    #region 저장해야 할 정보들
    public int m_iUnitLevel;
    public int m_iUnitEXP;
    public int m_iPermanentAttackMod;
    public int m_iPermanentDefMod;
    #endregion


    //인덱스로 실행하는게 편할거같아서 만들었습니다
    public SOAttackBase[] m_AttackBehaviors;

    //GameManager에 있는 UnitTable SO에 있는 정보를 기반으로 해당 게임오브젝트 초기화
    public void SetUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);

        //이름을 key값으로 받아온 value인 unitdata로 unitentity 초기화

        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = UnitData.m_iUnitLevel;

        //유닛의 최종 체력/공격력/방어력은 기본 + (레벨당능력치 * 레벨)입니다.
        //UnitDataTable의 정보를 토대로 공격력/방어력 설정
        m_iUnitHP = UnitData.m_iUnitHP + (UnitData.m_iLvlModHP * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (UnitData.m_iLvlModAtk * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (UnitData.m_iLvlModDef * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (UnitData.m_iLvlModSpeed * m_iUnitLevel);
        UnitType = UnitData.UnitType;
        m_iCurrentHP = m_iUnitHP;
        // 스프라이트 초기화
        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[3];
        m_iSkillAmounts = new int[3];

        //인덱스로 실행하는게 편할거같아서 만들었습니다
        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);
        for (int i = 0; i < 3; i++)
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;

        m_iPermanentDefMod = 0;
        m_iPermanentAttackMod = 0;
        m_iUnitDefMod_Temp = 0;
        m_iUnitAtkMod_Temp = 0;
    }

    //인덱스로 실행하는게 편할거같아서 만들었습니다
    public void AttackByIndex(UnitEntity Atker, UnitEntity Defender,int index)
    {
        m_AttackBehaviors[index].ExecuteAttack(Atker, Defender);
        m_iSkillAmounts[index] -= 1;
    }
    public string GetSkillname(UnitEntity UnitEntity,int index)
    {
        return UnitEntity.m_AttackBehaviors[index].GetSkillName();
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

    public void LevelUp()
    {
        var UnitData = GameManager.Instance.GetUnitData(m_sUnitName);
        m_iUnitHP += (UnitData.m_iLvlModHP);
        m_iCurrentHP += (UnitData.m_iLvlModHP);
        m_iUnitAtk += (UnitData.m_iLvlModAtk);
        m_iUnitDef += (UnitData.m_iLvlModDef);
        m_iUnitSpeed += (UnitData.m_iLvlModSpeed );
        m_iUnitEXP -= m_iUnitLevel * 10;
        m_iUnitLevel += 1;
    }

}
