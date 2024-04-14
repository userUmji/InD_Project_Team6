using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{

    public Sprite m_ItemSprite;
    public string m_sItemName;
    public string m_sItemDescription; // 임시로 생성한 설명 변수(엄지승)
    public IItemBehavior ExecuteEffect;
    public UnitEntity allyUnit;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        SetItemInfo();
    }

    public void SetItemInfo()
    {
        spriteRenderer = gameObject.transform.GetComponent<SpriteRenderer>();
        if (m_sItemName == null)
            Debug.Log("Item Name Missed");
        var ItemData = GameManager.Instance.GetItemData(m_sItemName);
        m_ItemSprite = ItemData.m_ItemSprite;
        allyUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].GetComponent<UnitEntity>();
        ExecuteEffect = Instantiate(ItemData.m_ItemEffect);
        spriteRenderer.sprite = m_ItemSprite;
    }

    public void ExecuteItem()
    {
        ExecuteEffect.ExecuteItemEffect(allyUnit, null);
    }
}
