using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    //적 유닛
    public GameObject g_EnemyUnit;
    public GameObject g_BattleButtons;
    public GameObject g_ChangeButton;

    public GameObject m_Canvas;
    // 전투 상태
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, SELECT, ENEMYTURN, RESULT, END }

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private bool isFall = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;
   

    // 플레이어 유닛
    public UnitEntity playerUnit;
    UnitEntity enemyUnit;

    // 전투 다이얼로그 텍스트
    public Text dialogueText;

    // 플레이어 / 적 HUD(Head-Up Display)
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // ���� ����

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.g_sEnemyBattleUnit = "더미";
        BattleInit();

        GameManager.Instance.g_InventoryGO.transform.SetParent(m_Canvas.transform);
        
    }
    #region 전투 메서드
    void BattleInit()
    {
        //전투 초기화
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName(GameManager.Instance.g_sEnemyBattleUnit);
        state = BattleState.START;
        BattleCoroutine = StartCoroutine(SetupBattle());
    }

    private void PlayerAction()
    {
        //state 설정
        state = BattleState.ACTION;
        dialogueText.text = playerUnit.m_sUnitName + "는 무엇을 할까..?";
    }
    #region 플레이어 Action 처리

    private void Process()
    {
        // 버튼에서 받은 액션의 종류
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
            if (playerUnit.m_iUnitSpeed + playerUnit.m_iTempSpeedMod + playerUnit.m_iPermanentSpeedMod > enemyUnit.m_iUnitSpeed)
                BattleCoroutine = StartCoroutine(PlayerTurn_Attack());
            else if (playerUnit.m_iUnitSpeed + playerUnit.m_iTempSpeedMod + playerUnit.m_iPermanentSpeedMod < enemyUnit.m_iUnitSpeed)
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
        {
            if (isFall == false)
                StartCoroutine(EnemyTurn());
            else
                StartCoroutine(Result());
        }
             
    }
    private void RunProcess()
    {
        if (state != BattleState.PLAYERTURN)
            StartCoroutine(PlayerTurn_Run());
        else
            StartCoroutine(EnemyTurn());
    }
    

    #endregion
    // 전투 종료 처리

    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "승리했다";
        StartCoroutine(PlayerWin());

    }
    void AfterFall()
    {
        state = BattleState.ACTION;
        int fallCount = 0;
        isFall = true;
        for(int i = 0; i< GameManager.Instance.m_UnitManager.g_PlayerUnits.Length;i++)
        {
            if (GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_iCurrentHP <= 0)
                fallCount += 1;
        }
        if(fallCount == 3)
        {
            StartCoroutine(PlayerLost());
        }
        else
            StartCoroutine(Player_FallChange());
    }
    //전투 패배 처리

    private void ChangeScene()
    {
        GameManager.Instance.Canvas_WorldScene.SetActive(true);
        GameManager.Instance.g_InventoryGO.transform.SetParent(GameManager.Instance.Canvas_WorldScene.transform);
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }

    public void UseItem()
    {
        GameManager.Instance.g_InventoryGO.transform.GetComponentInChildren<Inventory_Controller>().Hide_Inv();
        state = BattleState.PROCESS;
        m_ePlayerAction = GameManager.Action.ITEM;
        Process();
    }
        #endregion

    #region 전투 코루틴
    // 전투 셋업
    IEnumerator SetupBattle()
    {
        //플레이어 유닛 가져오기
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();

        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        //적 유닛 초기화
        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        enemyHUD.g_imagePortrait.sprite = enemyUnit.m_spriteUnitImage;

        dialogueText.text = "야생의" + enemyUnit.m_sUnitName + "이 나타났다...";

        // HUD 초기화
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // 대기
        yield return new WaitForSeconds(2f);

        // 플레이어 액션으로 진행
        PlayerAction();
    }
    IEnumerator Player_FallChange()
    {
        dialogueText.text = playerUnit.m_sUnitName + "는 쓰러졌다.....";
        yield return new WaitForSeconds(2f);
        isPlayed = false;
        g_ChangeButton.transform.GetComponent<BattleButtonCTR>().Onclick_Change();
    }
    #region 플레이어 액션 실행
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        if (playerUnit.g_UnitState == UnitEntity.UnitState.BERSERK)
        {
            dialogueText.text = playerUnit.m_sUnitName + "는 말을 듣지 않는다!";
            yield return new WaitForSeconds(1f);
            playerUnit.AttackByIndex(playerUnit, enemyUnit, Random.Range(0,playerUnit.m_AttackBehaviors.Length));
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = playerUnit.m_sUnitName + "의 " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " 공격!!";
        }
        else
        {
            playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = playerUnit.m_sUnitName + "의 " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " 공격!!";
        }
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

        dialogueText.text = "아이템을 사용했다!";
        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Change()
    {
        state = BattleState.PLAYERTURN;

        //unitManager의 instance를 가져옴
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];

        //playerUnit 설정
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        //UI 초기화
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        
        Process();
    }
    IEnumerator PlayerTurn_Run()
    {
        state = BattleState.PLAYERTURN;
        int runChance;
        if (playerUnit.m_iUnitLevel >= enemyUnit.m_iUnitLevel)
            runChance = 70;
        else
            runChance = 30;

        int randomChance = Random.Range(1, 101);
        Debug.Log(randomChance);
        if (runChance < randomChance)
        {
            dialogueText.text = "무사히 도망쳤다";
            yield return new WaitForSeconds(2f);
            ChangeScene();
        }
        else
        {
            dialogueText.text = "도망치지 못했다!";
            yield return new WaitForSeconds(2f);
            isPlayed = true;
            Process();
        }
    }
    #endregion
    IEnumerator EnemyTurn()
    {
        // state 변경
        state = BattleState.ENEMYTURN;
        // 랜덤 공격

        int randomAttackIndex = Random.Range(0, 2);
        enemyUnit.AttackByIndex(enemyUnit, playerUnit, randomAttackIndex);
        //적 공격
        string AttackName = enemyUnit.GetSkillname(enemyUnit, randomAttackIndex);
        playerHUD.SetHUD(playerUnit);
        dialogueText.text = enemyUnit.m_sUnitName + "의 " + AttackName + "공격!";


        // 1초 대기
        yield return new WaitForSeconds(1f);
        //Result로
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

    }

    IEnumerator Result()
    {
        dialogueText.text = "턴 실행 완료";
    
        yield return new WaitForSeconds(1f);
        if (playerUnit.m_iCurrentHP <= 0)
            AfterFall();
        else if (enemyUnit.m_iCurrentHP <= 0)
            AfterWin();
        else
        {
            state = BattleState.ACTION;
            isPlayed = false;
            isFall = false;
            PlayerAction();
        }
    }

    IEnumerator PlayerWin()
    {
        state = BattleState.END;
        dialogueText.text = "승리했다!";
        yield return new WaitForSeconds(1f);
        foreach(GameObject entity in GameManager.Instance.m_UnitManager.g_PlayerUnits)
        {
            float mod;
            UnitEntity unitEntity = entity.GetComponent<UnitEntity>();
            if (unitEntity.m_sUnitName == playerUnit.m_sUnitName)
                mod = 0.5f;
            else
                mod = 0.25f;
            int gainExpTemp = (int)(enemyUnit.m_iUnitLevel * 3 * mod);
            unitEntity.m_iUnitEXP += gainExpTemp;
            dialogueText.text = unitEntity.m_sUnitName + "는 " + gainExpTemp + "의 경험치를 얻었다.";
            yield return new WaitForSeconds(1f);
            while(unitEntity.m_iUnitEXP >= unitEntity.m_iUnitLevel * 10)
            {
                unitEntity.LevelUp();
                dialogueText.text = unitEntity.m_sUnitName + "는" + unitEntity.m_iUnitLevel +"로 레벨업했다!";
                playerHUD.levelText.text = "Lvl " + unitEntity.m_iUnitLevel ;
                yield return new WaitForSeconds(1f);
                dialogueText.text = unitEntity.m_sUnitName + "는 강해져서 기분이 좋은 것 같다!";
                yield return new WaitForSeconds(1f);
            }
        }
        ChangeScene();
    }
    
    IEnumerator PlayerLost()
    {
        dialogueText.text = "졌다";
        yield return new WaitForSeconds(2f);
        ChangeScene();
    }

    #endregion

    #region 버튼 처리
    // 버튼 처리
    public void OnButton(GameManager.Action action, int index)
    {
        // 액션 상태일때만 사용
        if (state != BattleState.ACTION)
            return;

        // 버튼 보이기
        g_BattleButtons.SetActive(true);
        //생성된 버튼 삭제
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
