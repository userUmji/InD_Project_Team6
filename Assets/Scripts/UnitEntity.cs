using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    public class UnitStateInfo
    {
        public UnitState state;
        public int stateDur;
        public int stateDamage;
    }
    public class UnitItemStateInfo
    {
        public int atk;
        public int def;
        public int spd;
        public int dur;
    }
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
    public int m_iStateDur;
    //public UnitState g_UnitState;
    public UnitStateInfo g_UnitState;


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
    public SOAttackBase[] m_AttackBehaviors;

    //GameManager에 있는 UnitData 정보로 초기화
    public void SetUnit(string className, int lvl)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);

        if(className == "일반 도깨비")
        {
            int randomType = Random.Range(0, 5);
            UnitType = (GameManager.Type)randomType;
            Debug.Log(randomType);
            Debug.Log(UnitType);
        }
        else
            UnitType = UnitData.UnitType;
        /*
        if (m_sUnitName == "일반 도깨비")
        {
            if (GameManager.Instance.g_Season == 0)
                UnitType = GameManager.Type.GODBEAST;
            else if (GameManager.Instance.g_Season == 1)
                UnitType = GameManager.Type.MONSTER;
            else if (GameManager.Instance.g_Season == 2)
                UnitType = GameManager.Type.FIRE;
            else if (GameManager.Instance.g_Season == 3)
                UnitType = GameManager.Type.ICE;
            else
                UnitType = GameManager.Type.GHOST;
        }
        else
            UnitType = UnitData.UnitType;
        */
        //UnitEntity 초기화
        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = lvl;

        m_iSkillAmounts = new int[4];

        m_iUnitHP = UnitData.m_iUnitHP + ((m_iUnitLevel - 5) * 2 * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (3 * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (3 * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (3 * m_iUnitLevel);
        
        m_iCurrentHP = m_iUnitHP;
        m_iUnitNo = UnitData.m_iUnitNo;

        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[4];
       
        List<SOAttackBase> attack_Temp = GameManager.Instance.Skills.FindAll(type => type.SkillType == UnitType);

        ChooseRandomAttack();
 
        m_AttackBehaviors[3] = Instantiate(UnitData.m_AttackBehav_Ult);
        for (int i = 0; i < 4; i++)
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;

        m_iPermanentDefMod = 0;
        m_iPermanentAtkMod = 0;
        m_iPermanentSpeedMod = 0;
        m_iTempAtkMod = 0;
        m_iTempDefMod = 0;
        m_iTempSpeedMod = 0;
        g_UnitState = new UnitStateInfo();
        g_UnitState.state = UnitState.NULL;
    }
    public void SetPlayerUnit(string className)
    {
        var UnitData = GameManager.Instance.GetUnitData(className);
        var UnitSaveData = GameManager.Instance.GetUnitSaveData(className);
        //UnitEntity 초기화
        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitLevel = UnitSaveData.m_iUnitLevel;

        m_iSkillAmounts = new int[4];

        m_iUnitHP = UnitData.m_iUnitHP + ((m_iUnitLevel - 5) * 2 * m_iUnitLevel);
        m_iUnitAtk = UnitData.m_iUnitAtk + (3 * m_iUnitLevel);
        m_iUnitDef = UnitData.m_iUnitDef + (3 * m_iUnitLevel);
        m_iUnitSpeed = UnitData.m_iUnitSpeed + (3 * m_iUnitLevel);
        UnitType = UnitData.UnitType;
        m_iCurrentHP = m_iUnitHP;
        m_iUnitNo = UnitData.m_iUnitNo;

        m_spriteUnitImage = UnitData.m_UnitSprite;
        m_AttackBehaviors = new SOAttackBase[4];

        m_AttackBehaviors[0] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_1]);
        m_AttackBehaviors[1] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_2]);
        m_AttackBehaviors[2] = Instantiate(GameManager.Instance.Skills[UnitSaveData.m_AttackBehav_3]);

        m_AttackBehaviors[3] = Instantiate(UnitData.m_AttackBehav_Ult);
        for (int i = 0; i < m_iSkillAmounts.Length; i++)
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
        g_UnitState = new UnitStateInfo();
        g_UnitState.state = UnitState.NULL;
        
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
        for (int i = 0; i < m_AttackBehaviors.Length - 1; i++)
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


        m_iUnitAtk += 3;
        m_iUnitDef += 3;
        m_iUnitSpeed += 3;
        m_iUnitEXP -= GameManager.Instance.g_iReqExp[m_iUnitLevel];
        m_iUnitLevel += 1;
        m_iUnitHP = UnitData.m_iUnitHP + ((m_iUnitLevel - 5) * 2 * m_iUnitLevel);
        m_iCurrentHP += (UnitData.m_iUnitHP + ((m_iUnitLevel - 5) * 2 * m_iUnitLevel))/10;
        GameManager.Instance.SavePlayerUnit(m_sUnitName, gameObject.transform.GetComponent<UnitEntity>());
    }
    public bool CheckLevelUP()
    {
        if (m_iUnitLevel > 50)
            return false;
        int exp_Max = GameManager.Instance.g_iReqExp[m_iUnitLevel];
        if (m_iUnitEXP > exp_Max)
            return true;
        return false;
    }
    public void ResetTempStatus()
    {
        m_iTempAtkMod = 0;
        m_iTempDefMod = 0;
        m_iTempSpeedMod = 0;
    }

    public void ResetUnit()
    {
        m_iCurrentHP = m_iUnitHP;
        for (int i = 0; i < m_iSkillAmounts.Length; i++)
        {
            m_iSkillAmounts[i] = m_AttackBehaviors[i].m_iUseAmount;
        }
    }

}
