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
    public GameObject g_Cursor;
    // 전투 상태
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, SELECT, ENEMYTURN, RESULT, END }

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private bool isFall = false;
    //버튼에서 전달받은 Action
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
            if (playerUnit.m_iUnitSpeed + playerUnit.m_iTempSpeedMod + playerUnit.m_iPermanentSpeedMod + playerUnit.m_AttackBehaviors[m_iPlayerActionIndex].m_iAdditionalSpeed > enemyUnit.m_iUnitSpeed)
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
        int unitCount = GameManager.Instance.m_UnitManager.CheckUnitAmount();
        isFall = true;
        for(int i = 0; i< unitCount;i++)
        {
            if (GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().m_iCurrentHP <= 0)
                fallCount += 1;
        }
        if(fallCount == unitCount)
        {
            StartCoroutine(PlayerLost());
        }
        else
            StartCoroutine(Player_FallChange());
    }
    //전투 패배 처리

    private void ChangeScene()
    {
        foreach(var Unit in GameManager.Instance.m_UnitManager.g_PlayerUnits)
        {
            if (Unit != null)
            {
                UnitEntity entity = Unit.transform.GetComponent<UnitEntity>();
                entity.m_iTempAtkMod = 0;
                entity.m_iTempDefMod = 0;
                entity.m_iTempSpeedMod = 0;
            }
        }
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

        if (enemyUnit.UnitType == GameManager.Type.GODBEAST)
            dialogueText.text = "앗! " + enemyUnit.m_sUnitName + "이다!!";
        dialogueText.text = "도깨비가 나타났다!";

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
        dialogueText.text = playerUnit.m_sUnitName + "이 쓰러졌다.....";
        yield return new WaitForSeconds(2f);
        dialogueText.text = "다음 도깨비를 선택하여 싸우자.";
        isPlayed = false;
        g_ChangeButton.transform.GetComponent<BattleButtonCTR>().Onclick_Change();
    }
    #region 플레이어 액션 실행
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        for (int i = 0; i < playerUnit.m_AttackBehaviors.Length; i++)
        {
            playerUnit.m_AttackBehaviors[i].m_isPlayed = false;
        }
        if (CalEffectChance(playerUnit,UnitEntity.UnitState.BERSERK))
        {
            dialogueText.text = playerUnit.m_sUnitName + "은 지금 광란상태다!";
            yield return new WaitForSeconds(1f);
            int randomIndex = Random.Range(0, playerUnit.m_AttackBehaviors.Length);
            playerUnit.AttackByIndex(playerUnit, enemyUnit, randomIndex);
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = playerUnit.m_sUnitName + "! " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " 공격!!";
            yield return new WaitForSeconds(1f);
            yield return CheckDouble(randomIndex, playerUnit, enemyUnit);
            yield return CheckEffected(randomIndex, playerUnit, enemyUnit);
        }
        else
        {
            if (CalEffectChance(playerUnit,UnitEntity.UnitState.PARALYSIS))
            {
                dialogueText.text = playerUnit.m_sUnitName + "은 지금 마비되었다.";
            }
            else
            {
                playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
                enemyHUD.SetHUD(enemyUnit);
                dialogueText.text = playerUnit.m_sUnitName + "의 " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " 공격!!";
                yield return new WaitForSeconds(1f);
                yield return CheckDouble(m_iPlayerActionIndex, playerUnit, enemyUnit);
                yield return CheckEffected(m_iPlayerActionIndex, playerUnit, enemyUnit);
            }
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
        dialogueText.text = "도깨비를 바꿨다.";
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
        if (runChance < randomChance)
        {
            dialogueText.text = "도깨비에게서 도망쳤다!";
            yield return new WaitForSeconds(2f);
            ChangeScene();
        }
        else
        {
            dialogueText.text = "앗! 도망에 실패했다.";
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
        dialogueText.text = enemyUnit.m_sUnitName + "이 " + AttackName + "공격을 했다!";
        yield return new WaitForSeconds(1f);
        yield return CheckDouble(randomAttackIndex, enemyUnit, playerUnit);
        yield return CheckEffected(randomAttackIndex, enemyUnit, playerUnit);

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
        TakeStateDamage(playerUnit);
        TakeStateDamage(enemyUnit);
    
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
        dialogueText.text = "도깨비를 처치했다!";
        yield return new WaitForSeconds(1f);
        int gainExpTemp;
        if (enemyUnit.m_iUnitLevel < 16)
            gainExpTemp = enemyUnit.m_iUnitLevel * 300;
        else if (enemyUnit.m_iUnitLevel >= 16 && enemyUnit.m_iUnitLevel < 30)
            gainExpTemp = enemyUnit.m_iUnitLevel * 250;
        else
            gainExpTemp = enemyUnit.m_iUnitLevel * 200;
        foreach (GameObject entity in GameManager.Instance.m_UnitManager.g_PlayerUnits)
        {
            if (entity != null)
            {
                UnitEntity unitEntity = entity.GetComponent<UnitEntity>();
                float mod;
                Debug.Log(GameManager.Instance.m_UnitManager.CheckUnitAmount());
                if(GameManager.Instance.m_UnitManager.CheckUnitAmount() == 1)
                {
                    mod = 1.0f;
                }
                else if (GameManager.Instance.m_UnitManager.CheckUnitAmount() == 2)
                {
                    if (unitEntity.m_sUnitName == playerUnit.m_sUnitName)
                        mod = 0.75f;
                    else
                        mod = 0.25f;
                }
                else
                {
                    if (unitEntity.m_sUnitName == playerUnit.m_sUnitName)
                        mod = 0.5f;
                    else
                        mod = 0.25f;
                }
                unitEntity.m_iUnitEXP += (int)(gainExpTemp * mod);
                dialogueText.text = unitEntity.m_sUnitName + "는 " + gainExpTemp * mod + "의 경험치를 얻었다.";
                yield return new WaitForSeconds(1f);
                bool isLevelUP = unitEntity.CheckLevelUP();
                while (isLevelUP)
                {
                    unitEntity.LevelUp();
                    dialogueText.text = unitEntity.m_sUnitName + "는" + unitEntity.m_iUnitLevel + "로 레벨업했다!";
                    playerHUD.levelText.text = "Lvl " + unitEntity.m_iUnitLevel;
                    yield return new WaitForSeconds(1f);
                    dialogueText.text = unitEntity.m_sUnitName + "는 강해져서 기분이 좋은 것 같다!";
                    yield return new WaitForSeconds(1f);
                    isLevelUP = unitEntity.CheckLevelUP();
                }
            }
        }
        ChangeScene();
    }
    
    IEnumerator PlayerLost()
    {
        dialogueText.text = "거대한 도깨비에게 져버렸다.....";
        yield return new WaitForSeconds(2f);
        ChangeScene();
    }

    #endregion
    #region 기타 메서드
    private WaitForSeconds CheckDouble(int index , UnitEntity Atker, UnitEntity Defer)
    {
        if (Atker.m_AttackBehaviors[index].m_IsDouble == 1)
        {
            dialogueText.text = Defer.m_sUnitName + "에게 치명적인듯 하다!";
            return new WaitForSeconds(1f);

        }
        else if (Atker.m_AttackBehaviors[index].m_IsDouble == 2)
        {
            dialogueText.text = Defer.m_sUnitName + "에게는 별로인듯 하다!";
            return new WaitForSeconds(1f);
        }
        else
            return new WaitForSeconds(0f);
    }

    private WaitForSeconds CheckEffected(int index, UnitEntity Atker, UnitEntity Defer)
    {
        if(Atker.m_AttackBehaviors[index].m_IsEffected == true)
        {
            dialogueText.text = StateEffect(index, Atker, Defer);
            return new WaitForSeconds(1f);
        }
        else
        {
            return new WaitForSeconds(0f);
        }
    }
    private string StateEffect(int index, UnitEntity Atker, UnitEntity Defer)
    {
        if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.SLOW)
            return Defer.m_sUnitName + "은 조금 느려진듯 하다";
        else if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.CARELESSNESS)
            return Defer.m_sUnitName + "은 조금 방심한듯 하다.";
        else if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.DEPRESSED)
            return Defer.m_sUnitName + "은 의욕을 상실한듯 하다.";
        else if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.HALF)
            return Defer.m_sUnitName + "일격!";
        else if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.ULTIMATE)
            return Defer.m_sUnitName + "은 최대의 힘을 냈다!";
        else if (Atker.m_AttackBehaviors[index].m_SkillEffect == SOAttackBase.SkillEffect.ONLYONCE)
            return "이 스킬은 연속으로 사용할 수 없다!";

        if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.FIRE && Atker.m_AttackBehaviors[index].m_iEffectDur == 3)
            return Defer.m_sUnitName + "의 몸에 불씨가 붙었다.";
        else if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.ICE && Atker.m_AttackBehaviors[index].m_iEffectDur == 3)
            return Defer.m_sUnitName + "의 몸에 얼음이 붙었다.";
        else if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.FIRE && Atker.m_AttackBehaviors[index].m_iEffectDur == 5)
            return Defer.m_sUnitName + "의 몸은 화상을 입었다.";
        else if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.ICE && Atker.m_AttackBehaviors[index].m_iEffectDur == 5)
            return Defer.m_sUnitName + "의 몸은 동상을 입었다.";
        else if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.PARALYSIS)
            return Defer.m_sUnitName + "는 마비되었다.";
        else if (Atker.m_AttackBehaviors[index].m_EffectState == UnitEntity.UnitState.BERSERK)
            return Defer.m_sUnitName + "는 광란에 빠졌다.";

        return "";
    }
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
    public void TakeStateDamage(UnitEntity entity)
    {
        if (entity.g_UnitState.state == UnitEntity.UnitState.FIRE || entity.g_UnitState.state == UnitEntity.UnitState.ICE)
        {
            entity.TakeDamage(entity.g_UnitState.stateDamage);
            dialogueText.text = entity.m_sUnitName + "는 " + entity.g_UnitState.stateDamage + "의 데미지를 받았다!";
        }
        if (entity.g_UnitState.stateDur != 0)
        {
            entity.g_UnitState.stateDur -= 1;
            if (entity.g_UnitState.stateDur == 0)
                entity.g_UnitState.state = UnitEntity.UnitState.NULL;
        }
    }
    public bool CalEffectChance(UnitEntity entity , UnitEntity.UnitState state)
    {
        if(entity.g_UnitState.state == state)
        {
            int attackChance = 30;
            int chance = Random.Range(0, 101);
            if (attackChance < chance)
                return true;
        }
        return false;
    }
    #endregion
    #endregion
}
