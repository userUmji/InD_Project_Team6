using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    // 플레이어와 적의 프리팹
    public GameObject[] g_PlayerUnits;
    public GameObject g_EnemyUnit;
    public GameObject g_BattleButtons;
    // 전투 상태를 정의하는 열거형
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;

    // 현재 플레이어와 적의 유닛의 스크립트
    public UnitEntity playerUnit;
    UnitEntity enemyUnit;

    // 전투 중 발생하는 대화를 표시하는 UI 텍스트
    public Text dialogueText;

    // 플레이어와 적의 HUD(Head-Up Display)를 관리하는 객체
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // 전투 상태

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        BattleInit();
    }

    #region 전투 관련 메서드
    void BattleInit()
    {
        //적
        //유닛 초기화
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName(GameManager.Instance.g_sEnemyBattleUnit);
        state = BattleState.START;
        BattleCoroutine = StartCoroutine(SetupBattle());
    }
    // 플레이어 턴 시작 처리   ----------------------------------------------------------------------------------------------------------------------
    private void PlayerAction()
    {
        //state 변경
        state = BattleState.ACTION;
        dialogueText.text = playerUnit.m_sUnitName + "는 어떻게 할 것 인가..";
    }
    #region 플레이어 Action 처리

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
    // 전투 종료 처리

    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "승리했다!";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }

    void AfterLost()
    {
        state = BattleState.END;

        dialogueText.text = "패배했다..";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }

    #endregion

    #region 전투 관련 코루틴
    // 전투 설정을 처리하는 코루틴
    IEnumerator SetupBattle()
    {
        //아군 유닛 초기화
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();

        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        //적 유닛 초기화
        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        enemyHUD.g_imagePortrait.sprite = enemyUnit.m_spriteUnitImage;


        // 대화 텍스트에 적의 이름을 표시
        dialogueText.text = "야생의 " + enemyUnit.m_sUnitName + " 이(가) 나타났다...";

        // 플레이어와 적의 HUD를 업데이트
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // 전투 설정 후 잠시 대기
        yield return new WaitForSeconds(2f);

        // 플레이어 턴으로 상태 전환
        PlayerAction();
    }
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        //공격 실행
        playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
        enemyHUD.SetHP(enemyUnit.m_iCurrentHP);
        dialogueText.text = playerUnit.m_sUnitName + "의 " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " 공격!!";
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

        // 플레이어의 체력을 HUD에 업데이트하고 대화 텍스트 표시
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "체력을 5 회복했다!";


        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Change()
    {
        state = BattleState.PLAYERTURN;

        //unitManager에 있는 유닛으로 교체
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];

        //playerUnit 지정
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        //UI 초기화
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }


    // 적의 턴을 처리하는 코루틴
    IEnumerator EnemyTurn()
    {
        // 상태 초기화
        state = BattleState.ENEMYTURN;
        // 적이 공격하고 대화 텍스트 업데이트

        int randomAttackIndex = Random.Range(0, 2);
        enemyUnit.AttackByIndex(enemyUnit, playerUnit, randomAttackIndex);
        //?띿뒪??泥섎━
        string AttackName = enemyUnit.GetSkillname(enemyUnit, randomAttackIndex);
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = enemyUnit.m_sUnitName + " 의 " + AttackName + "공격!";


        // 플레이어가 데미지를 받고 체력 업데이트
        yield return new WaitForSeconds(1f);
        //체력이 0보다 낮다면 Result로
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

    }
    // 플레이어 회복을 처리하는 코루틴

    IEnumerator Result()
    {
        dialogueText.text = "턴 실행 완료..";

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

    #region 버튼 클릭 이벤트
    // 공격 버튼 클릭 시 호출되는 메서드
    public void OnButton(GameManager.Action action, int index)
    {
        // 플레이어 턴이 아닌 경우에는 아무 작업도 수행하지 않음
        if (state != BattleState.ACTION)
            return;

        //배틀 UI 킴
        g_BattleButtons.SetActive(true);
        //생성했던 버튼 제거
        GameObject[] destroy = GameObject.FindGameObjectsWithTag("CreatedButtons");
        for (int i = 0; i < destroy.Length; i++)
            Destroy(destroy[i]);
        if (action == GameManager.Action.CANCLE)
            return;

        m_ePlayerAction = action;
        m_iPlayerActionIndex = index;


        state = BattleState.PROCESS;
        Process();
    }
    #endregion
}
