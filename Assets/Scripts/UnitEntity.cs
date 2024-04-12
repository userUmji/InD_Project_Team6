using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    public enum UnitState { NULL, ICE, FIRE, BERSERK, PARALYSIS};
    #region ���� ����
    public string m_sUnitName;
    public int m_iUnitHP;
    public int m_iUnitAtk;
    public int m_iUnitDef;
    public int m_iUnitSpeed;
    public int m_iCurrentHP;
    public int m_iTempAtkMod;
    public int m_iTempDefMod;
    public int m_iTempSpeedMod;
    public int[] m_iSkillAmounts;
    public UnitState g_UnitState;

    public Sprite m_spriteUnitImage;
    public GameManager.Type UnitType;
    #endregion
    #region �����ؾ� �� ������
    public int m_iUnitLevel;
    public int m_iUnitEXP;
    public int m_iPermanentAtkMod;
    public int m_iPermanentDefMod;
    public int m_iPermanentSpeedMod;
    public int m_iIntimacy;
    #endregion


    //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
    public SOAttackBase[] m_AttackBehaviors = new SOAttackBase[3];


   
    //GameManager�� �ִ� UnitTable SO�� �ִ� ������ ������� �ش� ���ӿ�����Ʈ �ʱ�ȭ
    public void SetUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);

        //�̸��� key������ �޾ƿ� value�� unitdata�� unitentity �ʱ�ȭ
        m_iSkillAmounts = new int[3];
        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = UnitData.m_iUnitLevel;

        //������ ���� ü��/���ݷ�/������ �⺻ + (������ɷ�ġ * ����)�Դϴ�.
        //UnitDataTable�� ������ ���� ���ݷ�/���� ����
        m_iUnitHP = UnitData.m_iUnitHP + (UnitData.m_iLvlModHP * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (UnitData.m_iLvlModAtk * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (UnitData.m_iLvlModDef * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (UnitData.m_iLvlModSpeed * m_iUnitLevel);
        UnitType = UnitData.UnitType;
        m_iCurrentHP = m_iUnitHP;
        // ��������Ʈ �ʱ�ȭ
        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[3];

        //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);
        for (int i = 0; i < 3; i++)
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;

        m_iPermanentDefMod = 0;
        m_iPermanentAtkMod = 0;
        m_iPermanentSpeedMod = 0;
        m_iTempAtkMod = 0;
        m_iTempDefMod = 0;
        m_iTempSpeedMod = 0;
        g_UnitState = UnitState.NULL;
    }

    //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
    public void AttackByIndex(UnitEntity Atker, UnitEntity Defender,int index)
    {
        m_AttackBehaviors[index].ExecuteAttack(Atker, Defender);
                m_iSkillAmounts[index] -= 1;
    }
    public string GetSkillname(UnitEntity UnitEntity,int index)
    {
        return UnitEntity.m_AttackBehaviors[index].GetSkillName();
    }

    // ü���� ȸ���ϴ� �޼���
    public void Heal(int amount)
    {
        // ȸ������ ���� ü�¿� ����
        m_iCurrentHP += amount;

        // ���� ���� ü���� �ִ� ü���� �ʰ��ϸ�
        if (m_iCurrentHP > m_iUnitHP)
            // ���� ü���� �ִ� ü������ ����
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
