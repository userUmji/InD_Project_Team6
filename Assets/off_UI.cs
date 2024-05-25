using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class off_UI : MonoBehaviour
{
    public TMP_InputField discard_value; // 버릴 개수 입력 받아오기
    public void Off_UI()
    {
        discard_value.text = "";
        transform.parent.gameObject.SetActive(false);
        Inventory_Controller.g_ICinstance.lock_UI = true;
    }
}
