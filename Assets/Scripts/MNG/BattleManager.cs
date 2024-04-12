using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    // �÷��̾�� ���� ������
    public GameObject[] g_PlayerUnits;
    public GameObject g_EnemyUnit;
    public GameObject g_BattleButtons;
    // ���� ���¸� �����ϴ� ������
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;

    // ���� �÷��̾�� ���� ������ ��ũ��Ʈ
    public UnitEntity playerUnit;
    UnitEntity enemyUnit;

    // ���� �� �߻��ϴ� ��ȭ�� ǥ���ϴ� UI �ؽ�Ʈ
    public Text dialogueText;

    // �÷��̾�� ���� HUD(Head-Up Display)�� �����ϴ� ��ü
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // ���� ����

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.g_sEnemyBattleUnit = "����";
        BattleInit();
    }
    #region ���� ���� �޼���
    void BattleInit()
    {
        //�� ���� �ʱ�ȭ
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName(GameManager.Instance.g_sEnemyBattleUnit);
        state = BattleState.START;
        BattleCoroutine = StartCoroutine(SetupBattle());
    }
    // �÷��̾� �� ���� ó��   ----------------------------------------------------------------------------------------------------------------------
    private void PlayerAction()
    {
        //state ����
        state = BattleState.ACTION;
        dialogueText.text = playerUnit.m_sUnitName + "�� ��� �� �� �ΰ�..";
    }
    #region �÷��̾� Action ó��

    private void Process()
    {
        // ��ư���� ������ �÷��̾� ���ǿ� ���� ���μ����� ����
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
    // ���� �¸� ó��

    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "�¸��ߴ�!";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }
    //���� �й� ó��
    void AfterLost()
    {
        state = BattleState.END;

        dialogueText.text = "�й��ߴ�..";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
    }

    #endregion

    #region ���� ���� �ڷ�ƾ
    // ���� ������ ó���ϴ� �ڷ�ƾ
    IEnumerator SetupBattle()
    {
        //�Ʊ� ���� �ʱ�ȭ
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();

        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        //�� ���� �ʱ�ȭ
        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        enemyHUD.g_imagePortrait.sprite = enemyUnit.m_spriteUnitImage;


        // ��ȭ �ؽ�Ʈ�� ���� �̸��� ǥ��
        dialogueText.text = "�߻��� " + enemyUnit.m_sUnitName + " ��(��) ��Ÿ����...";

        // �÷��̾�� ���� HUD�� ������Ʈ
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // ���� ���� �� ��� ���
        yield return new WaitForSeconds(2f);

        // �÷��̾� ������ ���� ��ȯ
        PlayerAction();
    }
    #region �÷��̾� �׼� ó��
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        //���� ����
        if (playerUnit.g_UnitState == UnitEntity.UnitState.BERSERK)
        {
            dialogueText.text = playerUnit.m_sUnitName + "�� ���� ���� �ʴ´�!";
            yield return new WaitForSeconds(1f);
            playerUnit.AttackByIndex(playerUnit, enemyUnit, Random.Range(0,playerUnit.m_AttackBehaviors.Length));
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = playerUnit.m_sUnitName + "�� " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " ����!!";
        }
        else
        {
            playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
            enemyHUD.SetHUD(enemyUnit);
            dialogueText.text = playerUnit.m_sUnitName + "�� " + playerUnit.GetSkillname(playerUnit, m_iPlayerActionIndex) + " ����!!";
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

        playerUnit.Heal(5);

        // �÷��̾��� ü���� HUD�� ������Ʈ�ϰ� ��ȭ �ؽ�Ʈ ǥ��
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "ü���� 5 ȸ���ߴ�!";


        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Change()
    {
        state = BattleState.PLAYERTURN;

        //unitManager�� �ִ� �������� ��ü
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];

        //playerUnit ����
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        //UI �ʱ�ȭ
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        isPlayed = true;
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
            dialogueText.text = "������ �����ƴ�";
            yield return new WaitForSeconds(2f);
            SceneManager.UnloadSceneAsync("BattleScene");
            GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
        }
        else
        {
            dialogueText.text = "����ġ�� ���ߴ�!";
            yield return new WaitForSeconds(2f);
            isPlayed = true;
            Process();
        }
    }
    #endregion
    // ���� ���� ó���ϴ� �ڷ�ƾ
    IEnumerator EnemyTurn()
    {
        // ���� �ʱ�ȭ
        state = BattleState.ENEMYTURN;
        // ���� �����ϰ� ��ȭ �ؽ�Ʈ ������Ʈ

        int randomAttackIndex = Random.Range(0, 2);
        enemyUnit.AttackByIndex(enemyUnit, playerUnit, randomAttackIndex);
        //���� ���� ����
        string AttackName = enemyUnit.GetSkillname(enemyUnit, randomAttackIndex);
        playerHUD.SetHUD(playerUnit);
        dialogueText.text = enemyUnit.m_sUnitName + " �� " + AttackName + "����!";


        // �÷��̾ �������� �ް� ü�� ������Ʈ
        yield return new WaitForSeconds(1f);
        //ü���� 0���� ���ٸ� Result��
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

    }
    // �÷��̾� ȸ���� ó���ϴ� �ڷ�ƾ

    IEnumerator Result()
    {
        dialogueText.text = "�� ���� �Ϸ�";
    
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

    IEnumerator PlayerWin()
    {
        state = BattleState.END;
        dialogueText.text = "�¸��ߴ�!";
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
            dialogueText.text = unitEntity.m_sUnitName + "�� " + gainExpTemp + "�� ����ġ�� �����.";
            yield return new WaitForSeconds(1f);
            while(unitEntity.m_iUnitEXP >= unitEntity.m_iUnitLevel * 10)
            {
                unitEntity.LevelUp();
                dialogueText.text = unitEntity.m_sUnitName + "��" + unitEntity.m_iUnitLevel +"������ �������ߴ�!";
                playerHUD.levelText.text = "Lvl " + unitEntity.m_iUnitLevel ;
                yield return new WaitForSeconds(1f);
                dialogueText.text = unitEntity.m_sUnitName + "�� �������� ����� ���� �� ����!";
                yield return new WaitForSeconds(1f);
            }
        }
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;

    }

    #endregion

    #region ��ư Ŭ�� �̺�Ʈ
    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnButton(GameManager.Action action, int index)
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.ACTION)
            return;

        //��Ʋ UI Ŵ
        g_BattleButtons.SetActive(true);
        //�����ߴ� ��ư ����
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
