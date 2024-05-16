using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemTable", menuName = "Tables/ItemTable")]
public class ItemTable : ScriptableObject
{
    [System.Serializable]
    public struct ItemStats
    {
        public int m_iItemNo;
        public string m_sItemName;
        public string m_sItemDescription;
        public string m_sItemUseDialog;
        public Sprite m_ItemSprite;


        public SOItemBase m_ItemEffect;
    }
    public ItemStats[] m_Items;
}
