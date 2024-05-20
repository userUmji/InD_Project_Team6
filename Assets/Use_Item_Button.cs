using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Use_Item_Button : MonoBehaviour
{
    public static GameObject g_gclick_item; // 사용할 아이템
    public int index;

    public void Use_Item()
    {
        g_gclick_item.GetComponent<Slot>().g_Ihave_item.ExecuteItem(index);
        g_gclick_item.GetComponent<Slot>().g_iitem_Number -= 1;
        g_gclick_item.GetComponent<Slot>().Refresh();
        if (g_gclick_item.GetComponent<Slot>().g_iitem_Number == 0)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        if(GameManager.Instance.g_GameState == GameManager.GameState.BATTLE)
        {
            BattleManager btm = GameObject.Find("BattleManager").transform.GetComponent<BattleManager>();
            btm.UseItem();
        }
    }
    private void OnEnable()
    {
        if (GameManager.Instance.m_UnitManager.g_PlayerUnits[index] != null)
        {
            gameObject.transform.GetComponent<Button>().interactable = true;
            gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>().m_sUnitName;
        }
        else
        {
            gameObject.transform.GetComponent<Button>().interactable = false;
            gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "X";
        }
    }
    public void InitName()
    {
        if(GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>() != null)
        {
            gameObject.transform.GetComponent<Button>().interactable = true;
            gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Instance.m_UnitManager.g_PlayerUnits[index].GetComponent<UnitEntity>().m_sUnitName;
        }
        else
        {
            gameObject.transform.GetComponent<Button>().interactable = false;
        }
    }
}