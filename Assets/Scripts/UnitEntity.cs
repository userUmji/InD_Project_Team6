using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    public enum UnitState { NULL, ICE, FIRE, BERSERK, PARALYSIS};
    #region 기본 정보
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
    public int m_iUnitNo;
    public UnitState g_UnitState;

    public Sprite m_spriteUnitImage;
    public GameManager.Type UnitType;
    #endregion
    #region 저장해야할 정보

    public int m_iUnitLevel;
    public int m_iUnitEXP;
    public int m_iPermanentAtkMod;
    public int m_iPermanentDefMod;
    public int m_iPermanentSpeedMod;
    public int m_iIntimacy;
    
    #endregion


    //공격 스킬 초기화
    public SOAttackBase[] m_AttackBehaviors = new SOAttackBase[3];
    public SOAttackBase m_AttackBehaviors_ult;



    //GameManager에 있는 UnitData 정보로 초기화
    public void SetUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);
        var UnitSaveData = GameManager.Instance.GetUnitSaveData(className);

        //UnitEntity 초기화
        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = UnitSaveData.m_iUnitLevel;

        m_iSkillAmounts = new int[4];

        m_iUnitHP = UnitData.m_iUnitHP + (UnitData.m_iLvlModHP * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (UnitData.m_iLvlModAtk * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (UnitData.m_iLvlModDef * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (UnitData.m_iLvlModSpeed * m_iUnitLevel);
        UnitType = UnitData.UnitType;
        m_iCurrentHP = m_iUnitHP;

        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[3];
        
        /*
        //랜덤 스킬 넣기를 위해 개발중
        List<SOAttackBase> attack_Temp = GameManager.Instance.Skills.FindAll(type => type.SkillType == UnitType);

        m_AttackBehaviors[0] = ChooseRandomAttack(attack_Temp);
        m_AttackBehaviors[1] = ChooseRandomAttack(attack_Temp);
        m_AttackBehaviors[2] = ChooseRandomAttack(attack_Temp);
        */
        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);
        for (int i = 0; i < 3; i++)
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;
        m_AttackBehaviors_ult = Instantiate(UnitData.m_AttackBehav_Ult);
        m_iSkillAmounts[3] = m_AttackBehaviors_ult.m_iUseAmount;

        m_iPermanentDefMod = 0;
        m_iPermanentAtkMod = 0;
        m_iPermanentSpeedMod = 0;
        m_iTempAtkMod = 0;
        m_iTempDefMod = 0;
        m_iTempSpeedMod = 0;
        g_UnitState = UnitState.NULL;
    }
    public void SetPlayerUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);
        var UnitSaveData = GameManager.Instance.GetUnitSaveData(className);
        //UnitEntity 초기화
        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = UnitSaveData.m_iUnitLevel;

        m_iSkillAmounts = new int[3];

        m_iUnitHP = UnitData.m_iUnitHP + (UnitData.m_iLvlModHP * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (UnitData.m_iLvlModAtk * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (UnitData.m_iLvlModDef * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (UnitData.m_iLvlModSpeed * m_iUnitLevel);
        UnitType = UnitData.UnitType;
        m_iCurrentHP = m_iUnitHP;

        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[3];

        //m_AttackBehaviors[0] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_1]);
        //m_AttackBehaviors[1] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_2]);
        //m_AttackBehaviors[2] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_3]);

        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);
        for (int i = 0; i < 3; i++)
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;

        //세이브데이터에서 불러오기
        m_iPermanentAtkMod = UnitSaveData.m_iPermanentAtkMod;
        m_iPermanentDefMod = UnitSaveData.m_iPermanentDefMod;
        m_iPermanentSpeedMod = UnitSaveData.m_iPermanentSpeedMod;
        m_iUnitEXP = UnitSaveData.m_iUnitEXP;
        m_iIntimacy = UnitSaveData.m_iIntimacy;

        m_iTempAtkMod = 0;
        m_iTempDefMod = 0;
        m_iTempSpeedMod = 0;
        g_UnitState = UnitState.NULL;
    }
    //공격 메서드
    public void AttackByIndex(UnitEntity Atker, UnitEntity Defender,int index)
    {
        m_AttackBehaviors[index].ExecuteAttack(Atker, Defender);
        m_iSkillAmounts[index] -= 1;
    }
    public string GetSkillname(UnitEntity UnitEntity,int index)
    {
        return UnitEntity.m_AttackBehaviors[index].GetSkillName();
    }

    // 힐 메서드
    public void Heal(int amount)
    {
        m_iCurrentHP += amount;
        if (m_iCurrentHP > m_iUnitHP)
            m_iCurrentHP = m_iUnitHP;
    }

    public void ChooseRandomAttack()
    {
        List<SOAttackBase> list = GameManager.Instance.Skills.FindAll(type => type.SkillType == UnitType);
        for (int i = 0; i < m_AttackBehaviors.Length; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            SOAttackBase temp = Instantiate(list[randomIndex]);
            list.RemoveAt(randomIndex);
            m_AttackBehaviors[i] = temp;
        }
    }

    public void TakeDamage(int amount)
    {
        m_iCurrentHP -= amount;
        if (m_iCurrentHP < 0)
            m_iCurrentHP = 0;
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
