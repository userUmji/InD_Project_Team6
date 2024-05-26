using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleButtonCTR : MonoBehaviour
{
    public GameObject BattleButtons;
    public GameObject SkillButtonPrefab;
    public GameObject g_Canvas;
    public BattleManager g_BattleManager;
    public GameManager.Action g_eAction;

    public int g_iIndex;

    // 델리게이트 선언
    delegate void OnButton(GameManager.Action action, int index);


    private void Start()
    {
        BattleButtons = gameObject.transform.parent.gameObject;
        g_Canvas = GameObject.Find("Canvas");
        g_BattleManager = GameObject.Find("BattleManager").transform.GetComponent<BattleManager>();

        if(g_eAction == GameManager.Action.RUN)
        {
            //BattleManager 할당
            g_BattleManager = GameObject.Find("BattleManager").transform.GetComponent<BattleManager>();
            // 델리게이트를 생성하고 할당
            OnButton buttonDelegate = new OnButton(g_BattleManager.OnButton);

            // Button 컴포넌트의 onClick 이벤트에 델리게이트 등록
            gameObject.GetComponent<Button>().onClick.AddListener(() => buttonDelegate(g_eAction, g_iIndex));
        }
    }
    public void OnClick_Skill()
    {
        if (g_BattleManager.state == BattleManager.BattleState.ACTION)
        {
            SkillButtonPrefab = Resources.Load<GameObject>("Prefabs/SkillButtons");

            GameObject SkillButton_Temp = Instantiate(SkillButtonPrefab, g_Canvas.transform);

            GameObject skillButtons = SkillButton_Temp.transform.GetChild(0).gameObject;
            for (int i = 0; i < skillButtons.transform.childCount; i++)
            {
                skillButtons.transform.GetChild(i).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = g_BattleManager.playerUnit.m_AttackBehaviors[i].GetSkillName();
                skillButtons.transform.GetChild(i).GetChild(2).transform.GetComponent<TextMeshProUGUI>().text = g_BattleManager.playerUnit.m_iSkillAmounts[i] + "/" + g_BattleManager.playerUnit.m_AttackBehaviors[i].m_iUseAmount;
                if (g_BattleManager.playerUnit.m_iSkillAmounts[i] == 0)
                {
                    skillButtons.transform.GetChild(i).transform.GetComponent<Button>().interactable = false;
                    skillButtons.transform.GetChild(i).GetChild(1).transform.GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0);
                    skillButtons.transform.GetChild(i).GetChild(2).transform.GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0);
                }
                if (g_BattleManager.playerUnit.m_AttackBehaviors[i].m_SkillEffect == SOAttackBase.SkillEffect.ONLYONCE && g_BattleManager.playerUnit.m_AttackBehaviors[i].m_isPlayed == true)
                    skillButtons.transform.GetChild(i).transform.GetComponent<Button>().interactable = false;

            }
            InitButton_ult(skillButtons);
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            BattleButtons.SetActive(false);
        }
    }
    public void Onclick_Change()
    {
        if (g_BattleManager.state == BattleManager.BattleState.ACTION)
        {
            //Debug.Log("변경 버튼 눌림");
            //프리펩을 Load
            GameObject ChangeButtonPrefab = Resources.Load<GameObject>("Prefabs/ChangeButtons");
            //instantiate 하고 텍스트를 바꾸기 위해 Temp에 저장
            GameObject ChangeButton_Temp = Instantiate(ChangeButtonPrefab, g_Canvas.transform);
            //버튼의 텍스트를 플레이어의 유닛 이름으로 바꿈
            for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
            {
                DIcElementCTR element =  ChangeButton_Temp.transform.GetChild(0).transform.GetChild(i).transform.GetComponent<DIcElementCTR>();
                element.Init(GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName);
                //ü���� 0�̰ų� �̹� �����ִ� �����̸�
                if (GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_iCurrentHP <= 0 
                    || GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_sUnitName == g_BattleManager.playerUnit.m_sUnitName)
                    ChangeButton_Temp.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().interactable = false;
                
            }
            for (int i = 0; i < GameManager.Instance.m_UnitManager.g_PlayerUnits.Length; i++)
                if (GameManager.Instance.m_UnitManager.g_PlayerUnits[i] == null)
                    ChangeButton_Temp.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            BattleButtons.SetActive(false);
        }
    }
    public void OnClick_Inventory()
    {
        if (g_BattleManager.state == BattleManager.BattleState.ACTION)
        {
            GameManager.Instance.GetInventoryGO().transform.GetComponentInChildren<Inventory_Controller>().Show_Inv();
            g_BattleManager.state = BattleManager.BattleState.SELECT;
        }
    }
    private void InitButton_ult(GameObject Buttons)
    {
        Buttons.transform.GetChild(3).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = g_BattleManager.playerUnit.m_AttackBehaviors[3].GetSkillName();
        Buttons.transform.GetChild(3).GetChild(2).transform.GetComponent<TextMeshProUGUI>().text = g_BattleManager.playerUnit.m_iSkillAmounts[3] + "/" + g_BattleManager.playerUnit.m_AttackBehaviors[3].m_iUseAmount;
        if (g_BattleManager.playerUnit.m_iSkillAmounts[3] == 0 || g_BattleManager.playerUnit.m_iIntimacy != 10)
        {
            if (g_BattleManager.playerUnit.m_iIntimacy != 10)
                Buttons.transform.GetChild(3).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "???";
            Buttons.transform.GetChild(3).transform.GetComponent<Button>().interactable = false;
            Buttons.transform.GetChild(3).GetChild(1).transform.GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0);
            Buttons.transform.GetChild(3).GetChild(2).transform.GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0);
        }
    }
    public void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
        g_BattleManager.g_Cursor.transform.position = gameObject.transform.position + new Vector3(155.0f, 0, 0);
    }
    public void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}


