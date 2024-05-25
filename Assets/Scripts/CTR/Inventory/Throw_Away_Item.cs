using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Throw_Away_Item : MonoBehaviour
{
    public TMP_InputField discard_value; // 버릴 개수 입력 받아오기
    public int discard_count;
    public void Throw_Item() // 인벤토리에서 클릭한 아이템 삭제
    {
        discard_count = int.Parse(discard_value.GetComponent<TMP_InputField>().text);
        if(Inventory_Controller.g_ICinstance.g_Sclick_Slot.g_iitem_Number < discard_count)
        {
           // Inventory_Controller.g_ICinstance.lock_UI = true;
            print("소지중인 아이템보다 버릴 아이템의 개수가 더 많습니다.");
        }
        else
        {
            Inventory_Controller.g_ICinstance.g_Sclick_Slot.g_iitem_Number -= discard_count;
            
        }
        
        if(Inventory_Controller.g_ICinstance.g_Sclick_Slot.g_iitem_Number != 0)
        {
            Inventory_Controller.g_ICinstance.g_Sclick_Slot.Refresh();
        }
        discard_value.GetComponent<TMP_InputField>().text = " ";
        discard_count = 0;
        gameObject.transform.parent.gameObject.SetActive(false);
        Inventory_Controller.g_ICinstance.lock_UI = true;
    }
}
