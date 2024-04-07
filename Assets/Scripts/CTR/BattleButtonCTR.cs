using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleButtonCTR : MonoBehaviour
{
    public GameObject BattleButtons;
    public GameObject SkillButtonPrefab;
    public GameObject g_Canvas;
    public BattleManager g_BattleManager;
    private void Start()
    {
        BattleButtons = gameObject.transform.parent.gameObject;
        g_Canvas = GameObject.Find("Canvas");
        g_BattleManager = GameObject.Find("BattleManager").transform.GetComponent<BattleManager>();
    }
    public void OnClick_Skill()
    {
        if (g_BattleManager.state == BattleManager.BattleState.ACTION)
        {
            Debug.Log("스킬버튼 눌림");
            SkillButtonPrefab = Resources.Load<GameObject>("Prefabs/SkillButtons");
            GameObject SkillButton_Temp = Instantiate(SkillButtonPrefab, g_Canvas.transform);
            for (int i = 0; i < SkillButton_Temp.transform.childCount - 1; i++)
            {
                SkillButton_Temp.transform.GetChild(i).GetChild(0).transform.GetComponent<Text>().text = g_BattleManager.playerUnit.m_AttackBehaviors[i].GetSkillName();
            }

            BattleButtons.SetActive(false);
        }
    }
    public void Onclick_Change()
    {
        if (g_BattleManager.state == BattleManager.BattleState.ACTION)
        {
            Debug.Log("변경 버튼 눌림");
            //프리펩을 Load
            GameObject ChangeButtonPrefab = Resources.Load<GameObject>("Prefabs/ChangeButtons");
            //instantiate 하고 텍스트를 바꾸기 위해 Temp에 저장
            GameObject ChangeButton_Temp = Instantiate(ChangeButtonPrefab, g_Canvas.transform);
            //버튼의 텍스트를 플레이어의 유닛 이름으로 바꿈
            for (int i = 0; i < GameManager.Instance.m_UnitManager.g_PlayerUnits.Length; i++)
                ChangeButton_Temp.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<Text>().text = GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName;
            BattleButtons.SetActive(false);
        }
    }
    public void OnClick_Inventory()
    {
        Debug.Log("인벤토리 버튼 눌림");
    }
    public void OnClick_Run()
    {
        Debug.Log("도망 눌림");
    }
}
