using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    public int number; // 엄지승이 임시로 구현한 아이템 번호입니다. 이후 변경 또는 수정 부탁드립니다.
    public Sprite m_ItemSprite;
    public string m_sItemName;
    public string m_sItemDescription;
    public string m_sItemUseDialog;
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
        m_sItemDescription = ItemData.m_sItemDescription;
        m_sItemUseDialog = ItemData.m_sItemUseDialog;
        ExecuteEffect = Instantiate(ItemData.m_ItemEffect);
        spriteRenderer.sprite = m_ItemSprite;
    }

    public void ExecuteItem(int index)
    {
        ExecuteEffect.ExecuteItemEffect(GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>());
    }

    public string GetUseDialog()
    {
        return m_sItemUseDialog;
    }
}
