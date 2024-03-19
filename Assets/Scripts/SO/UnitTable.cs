using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewUnitTable", menuName = "Tables/UnitTable")]
public class UnitTable : ScriptableObject {
    //Unit의 정보들을 담은 SO, 여기에 파라미터를 추가하면 SOInstance에서 수정하고 저장할수 있습니다. 

    [System.Serializable]
    public struct UnitStats
    {
        public string m_sUnitName;
        public string m_sUnitType;
        public Sprite m_UnitSprite;
        public int m_iUnitHP;
        public int m_iUnitAtk;
        public int m_iUnitSpeed;
        public int m_iUnitDef;
        public int m_iUnitLevel;
        

        public SOAttackBase m_AttackBehav_1;
        public SOAttackBase m_AttackBehav_2;
        public SOAttackBase m_AttackBehav_3;
    }


    public UnitStats[] m_Units;
}
