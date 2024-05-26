using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_UI : MonoBehaviour
{
    [Header("«•Ω√«ÿ¡Ÿ UI")]
    public GameObject change_UI;

    public GameObject[] off_UI;

    public static Change_UI instance;

    private void Awake()
    {
        instance = this;
    }
    public void Change_UI_Button()
    {
        if (GameManager.Instance.g_GameState == GameManager.GameState.BATTLE)
            return;
        for(int i = 0; i < off_UI.Length; i++)
        {
            if (off_UI[i].name == "Inventory")
            {
                off_UI[i].transform.localScale = new Vector3(0, 0, 1);

            }
            else
            {
                off_UI[i].SetActive(false);
            }
        }

        if (change_UI.name == "Inventory")
        {
            change_UI.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            change_UI.SetActive(true);
        }
    }

    public void ALL_OFF_UI() // ∏µÁ UI ≤®¡‹
    {
        for (int i = 0; i < off_UI.Length; i++)
        {
            if (off_UI[i].name == "Inventory")
            {
                off_UI[i].transform.localScale = new Vector3(0, 0, 1);
            }
            else
            {
                off_UI[i].SetActive(false);
            }
        }
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }
}
