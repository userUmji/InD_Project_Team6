using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 전투 상태를 정의하는 열거형
public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }


public class BattleManager : MonoBehaviour
{
    // 플레이어와 적의 프리팹
    public GameObject[] g_PlayerUnits;
    public GameObject g_EnemyUnit;

    // 플레이어와 적이 전투하는 위치
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform waitStation;

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;

    // 현재 플레이어와 적의 유닛의 스크립트
    UnitEntity playerUnit;
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

    private void Update()
    {
        //Debug.Log(state);
    }


    #region 전투 관련 메서드
    void BattleInit()
    {
        //플레이어 유닛 초기화
        //g_PlayerUnits = GameManager.Instance.m_UnitManager.g_PlayerUnits;
        //적 유닛 초기화
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName("개굴닌자");

        
        state = BattleState.START;
        // 전투 시작 상태로 초기화하고, 전투를 설정하는 코루틴 실행
        BattleCoroutine = StartCoroutine(SetupBattle());
    }





    // 플레이어 턴 시작 처리   ----------------------------------------------------------------------------------------------------------------------
    private void PlayerAction()
    {
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
        dialogueText.text = "승리!";
    }

    void AfterLost()
    {
        state = BattleState.END;
        dialogueText.text = ".......당신은 눈앞이 깜깜해졌다.";
    }

    #endregion

    #region 전투 관련 코루틴
    // 전투 설정을 처리하는 코루틴
    IEnumerator SetupBattle()
    {
        // 플레이어와 적의 유닛을 생성하고 배치
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();
        for(int i = 1; i< GameManager.Instance.m_UnitManager.g_PlayerUnits.Length;i++)
            GameManager.Instance.m_UnitManager.g_PlayerUnits[i].transform.position = waitStation.position;
        

        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        enemyUnit.transform.position = enemyBattleStation.position;
        playerUnit.transform.position = playerBattleStation.position;

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
    #region 플레이어 Action 처리
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        //공격 실행
        playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
        enemyHUD.SetHP(enemyUnit.m_iCurrentHP);
        dialogueText.text = playerUnit.m_sUnitName + "의 공격!!";
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

        // 플레이어 교체
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];
        playerUnit.transform.position = waitStation.position; // 이전 플레이어 이동
        newPlayerGO.transform.position = playerBattleStation.position;
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();


        // 플레이어의 체력을 HUD에 업데이트
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    #endregion

    // 적의 턴을 처리하는 코루틴
    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        // 적이 공격하고 대화 텍스트 업데이트
        int randomAttackIndex = Random.Range(0, 2);
        string AttackName = enemyUnit.AttackByIndex(enemyUnit, playerUnit,randomAttackIndex);
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = enemyUnit.m_sUnitName + " 의 " + AttackName + "공격!";


        // 플레이어가 데미지를 받고 체력 업데이트

        yield return new WaitForSeconds(1f);
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
        m_ePlayerAction = action;
        m_iPlayerActionIndex = index;


        state = BattleState.PROCESS;
        Process();
    }
    #endregion
}
