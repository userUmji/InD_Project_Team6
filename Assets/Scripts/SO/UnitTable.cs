using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewUnitTable", menuName = "Tables/UnitTable")]
public class UnitTable : ScriptableObject {
    //UnitTable에서 작성할 항목

    [System.Serializable]
    public struct UnitStats
    {
        public int m_iUnitNo;
        public string m_sUnitName;
        public string m_sUnitExplain;
        public GameManager.Type UnitType;
        public Sprite m_UnitSprite;
        public int m_iUnitHP;
        public int m_iUnitAtk;
        public int m_iUnitSpeed;
        public int m_iUnitDef;
        //public int m_iUnitLevel;

        public SOAttackBase m_AttackBehav_1;
        public SOAttackBase m_AttackBehav_2;
        public SOAttackBase m_AttackBehav_3;
        public SOAttackBase m_AttackBehav_Ult;
    }
    public class UnitStats_Save
    {
   
        public bool m_isCaptured;
        public int m_iUnitLevel;
        public int m_iUnitEXP;
        public int m_iPermanentAtkMod;
        public int m_iPermanentDefMod;
        public int m_iPermanentSpeedMod;
        public int m_iIntimacy;
        public int m_AttackBehav_1;
        public int m_AttackBehav_2;
        public int m_AttackBehav_3;
        public int m_AttackBehav_Ult;
    }


    public UnitStats[] m_Units;
}
