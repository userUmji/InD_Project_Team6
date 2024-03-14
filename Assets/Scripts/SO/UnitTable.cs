using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewUnitTable", menuName = "Tables/UnitTable")]
public class UnitTable : ScriptableObject {


    [System.Serializable]
    public struct UnitStats
    {
        public string m_sUnitName;
        public Sprite m_UnitSprite;
        public int m_iHealthPoint;
        public int m_iAttackDamage;
        public int m_iUnitSpeed;
        public int m_iUnitLevel;

        public SOAttackBase m_AttackBehav_1;
        public SOAttackBase m_AttackBehav_2;
        public SOAttackBase m_AttackBehav_3;
        public SOAttackBase m_AttackBehav_4;
    }


    public UnitStats[] m_Units;
}
