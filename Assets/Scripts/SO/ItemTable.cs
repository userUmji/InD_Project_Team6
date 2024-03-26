using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemTable", menuName = "Tables/ItemTable")]
public class ItemTable : ScriptableObject
{
    [System.Serializable]
    public struct ItemStats
    {
        public string m_sItemName;
        public string m_sItemDescription;
        public Sprite m_ItemSprite;


        public SOItemBase m_ItemEffect;
    }
    public ItemStats[] m_Items;
}
