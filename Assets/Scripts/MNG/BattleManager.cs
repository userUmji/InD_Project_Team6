using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BattleManager : MonoBehaviour
{
    public GameObject[] g_PlayerUnits;
    public GameObject g_EnemyUnit;
    public GameObject g_BattleButtons;
    // 전투 상태를 정의하는 열거형
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }

    // �댁댁 � �ы 移
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform waitStation;

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;


    // 현재 플레이어와 적의 유닛의 스크립트
    public UnitEntity playerUnit;
    UnitEntity enemyUnit;

    // � 以 諛 瑜  UI ㅽ
    public Text dialogueText;

    // �댁댁 � HUD(Head-Up Display)瑜 愿由ы 媛泥
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // � 
    public BattleState state;

    // Start is called before the first frame update
    void Start()

    {
        BattleInit();
    }

    private void Update()
    {
        //Debug.Log(state);
    }


    #region � 愿� 硫
    void BattleInit()
    {
        //�댁  珥湲고
        //g_PlayerUnits = GameManager.Instance.m_UnitManager.g_PlayerUnits;
        //�  珥湲고
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName("媛援대");

        
        state = BattleState.START;
        // �  濡 珥湲고怨, �щ� ㅼ 肄猷⑦ ㅽ
        BattleCoroutine = StartCoroutine(SetupBattle());
    }





    // �댁   泥由   ----------------------------------------------------------------------------------------------------------------------
    private void PlayerAction()
    {
        state = BattleState.ACTION;
        dialogueText.text = playerUnit.m_sUnitName + " 대산  寃 멸..";
    }
    #region �댁 Action 泥由
    private void Process()
    {
        if (m_ePlayerAction == GameManager.Action.ATTACK)
            AttackProcess();
        else if (m_ePlayerAction == GameManager.Action.ITEM)
            ItemProcess();
        else if (m_ePlayerAction == GameManager.Action.CHANGE)
            ChangeProcess();
        else if (m_ePlayerAction == GameManager.Action.RUN)
            RunProcess();
    }
    private void AttackProcess()
    {
        if (state != BattleState.ENEMYTURN && state != BattleState.PLAYERTURN)
        {
            if (playerUnit.m_iUnitSpeed > enemyUnit.m_iUnitSpeed)
                BattleCoroutine = StartCoroutine(PlayerTurn_Attack());
            else if (playerUnit.m_iUnitSpeed < enemyUnit.m_iUnitSpeed)
                BattleCoroutine = StartCoroutine(EnemyTurn());
            else
            {
                if (playerUnit.m_iUnitLevel < enemyUnit.m_iUnitLevel)
                    StartCoroutine(EnemyTurn());
                else
                    StartCoroutine(PlayerTurn_Attack());
            }
        }
        else if (state == BattleState.ENEMYTURN)
            StartCoroutine(PlayerTurn_Attack());
        else if (state == BattleState.PLAYERTURN)
            StartCoroutine(EnemyTurn());
    }
    private void ItemProcess()
    {
        if (state != BattleState.PLAYERTURN)
            StartCoroutine(PlayerTurn_Item());
        else
            StartCoroutine(EnemyTurn());
    }
    private void ChangeProcess()
    {
        if (state != BattleState.PLAYERTURN)
            StartCoroutine(PlayerTurn_Change());
        else
            StartCoroutine(EnemyTurn());
    }
    private void RunProcess()
    {
        if (state != BattleState.PLAYERTURN)
            StartCoroutine(PlayerTurn_Item());
        else
            StartCoroutine(EnemyTurn());
    }

    #endregion


    // � 醫猷 泥由
    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "밸━!";
    }

    void AfterLost()
    {
        state = BattleState.END;
        dialogueText.text = ".......뱀  源源댁.";
    }

    #endregion

    #region � 愿� 肄猷⑦
    // � ㅼ 泥由ы 肄猷⑦
    IEnumerator SetupBattle()
    {
        // �댁댁 �  깊怨 諛곗
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();
        //UI의 이미지로 스프라이트를 지정하기 때문에 Station들을 삭제했습니다.
        //for(int i = 1; i< GameManager.Instance.m_UnitManager.g_PlayerUnits.Length;i++)
            //GameManager.Instance.m_UnitManager.g_PlayerUnits[i].transform.position = waitStation.position;
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;

        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        //enemyUnit.transform.position = enemyBattleStation.position;
        //playerUnit.transform.position = playerBattleStation.position];
        enemyHUD.g_imagePortrait.sprite = enemyUnit.m_spriteUnitImage;

        //  ㅽ몄 � 대 
        dialogueText.text = "쇱 " + enemyUnit.m_sUnitName + " (媛) щ...";

        // �댁댁 � HUD瑜 곗댄
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // � ㅼ   湲
        yield return new WaitForSeconds(2f);

        // �댁 댁쇰  �
        PlayerAction();
    }
    #region �댁 Action 泥由
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        //怨듦꺽 ㅽ
        playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
        enemyHUD.SetHP(enemyUnit.m_iCurrentHP);

        dialogueText.text = playerUnit.m_sUnitName + "의 " + playerUnit.GetSkillname(playerUnit,m_iPlayerActionIndex)+" 공격!!";
        yield return new WaitForSeconds(1f);
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Item()
    {
        state = BattleState.PLAYERTURN;

        playerUnit.Heal(5);

        // �댁댁 泥대μ HUD 곗댄명怨  ㅽ 
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "泥대μ 5 蹂듯!";

        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Change()
    {
        state = BattleState.PLAYERTURN;

        // �댁 援泥
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];

        //playerUnit.transform.position = waitStation.position; // 이전 플레이어 이동
        //newPlayerGO.transform.position = playerBattleStation.position;
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;


        // �댁댁 泥대μ HUD 곗댄
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    #endregion

    // � 댁 泥由ы 肄猷⑦
    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        // � 怨듦꺽怨  ㅽ 곗댄
        int randomAttackIndex = Random.Range(0, 2);
        enemyUnit.AttackByIndex(enemyUnit, playerUnit, randomAttackIndex);
        string AttackName = enemyUnit.GetSkillname(enemyUnit,randomAttackIndex);
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = enemyUnit.m_sUnitName + "  " + AttackName + "怨듦꺽!";


        // �댁닿 곕�吏瑜 諛怨 泥대 곗댄

        yield return new WaitForSeconds(1f);
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

    }
    // �댁 蹂듭 泥由ы 肄猷⑦


    IEnumerator Result()
    {
        dialogueText.text = " ㅽ 猷..";
        yield return new WaitForSeconds(1f);
        if (playerUnit.m_iCurrentHP <= 0)
            AfterLost();
        else if (enemyUnit.m_iCurrentHP <= 0)
            AfterWin();
        else
        {
            state = BattleState.ACTION;
            isPlayed = false;
            PlayerAction();
        }

    }

    #endregion

    #region 踰 대┃ 대깽
    // 怨듦꺽 踰 대┃  몄 硫
    public void OnButton(GameManager.Action action, int index)
    {
        // �댁 댁  寃쎌곗 臾  吏 
        if (state != BattleState.ACTION)
            return;
        m_ePlayerAction = action;
        m_iPlayerActionIndex = index;
        g_BattleButtons.SetActive(true);

        GameObject[] destroy = GameObject.FindGameObjectsWithTag("CreatedButtons");
        for (int i = 0; i< destroy.Length;i++)
        {
            Destroy(destroy[i]);
        }
        
        

        state = BattleState.PROCESS;
        Process();
    }
    #endregion
}
