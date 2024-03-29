using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    // ÇÃ·¹ÀÌ¾î¿Í ÀûÀÇ ÇÁ¸®ÆÕ
    public GameObject[] g_PlayerUnits;
    public GameObject g_EnemyUnit;
    public GameObject g_BattleButtons;
    // ÀüÅõ »óÅÂ¸¦ Á¤ÀÇÇÏ´Â ¿­°ÅÇü
    public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }

<<<<<<< Updated upstream
    // ÇÃ·¹ÀÌ¾î¿Í ÀûÀÌ ÀüÅõÇÏ´Â À§Ä¡
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform waitStation;
=======
>>>>>>> Stashed changes

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;

    // ÇöÀç ÇÃ·¹ÀÌ¾î¿Í ÀûÀÇ À¯´ÖÀÇ ½ºÅ©¸³Æ®
    public UnitEntity playerUnit;
    UnitEntity enemyUnit;

<<<<<<< Updated upstream
    // ÀüÅõ Áß ¹ß»ıÇÏ´Â ´ëÈ­¸¦ Ç¥½ÃÇÏ´Â UI ÅØ½ºÆ®
    public Text dialogueText;

    // ÇÃ·¹ÀÌ¾î¿Í ÀûÀÇ HUD(Head-Up Display)¸¦ °ü¸®ÇÏ´Â °´Ã¼
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // ÀüÅõ »óÅÂ
=======
    //ë‹¤ì´ì–¼ë¡œê·¸ í…ìŠ¤íŠ¸
    public Text dialogueText;

  //í”Œë ˆì´ì–´ì™€ ì  UI
    public BattleHUDCTR playerHUD;
    public BattleHUDCTR enemyHUD;

    // ì „íˆ¬ ìƒíƒœ
>>>>>>> Stashed changes
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        BattleInit();
    }


<<<<<<< Updated upstream

    #region ÀüÅõ °ü·Ã ¸Ş¼­µå
    void BattleInit()
    {
        //ÇÃ·¹ÀÌ¾î À¯´Ö ÃÊ±âÈ­
        //g_PlayerUnits = GameManager.Instance.m_UnitManager.g_PlayerUnits;
        //Àû À¯´Ö ÃÊ±âÈ­
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName("°³±¼´ÑÀÚ");

        
        state = BattleState.START;
        // ÀüÅõ ½ÃÀÛ »óÅÂ·Î ÃÊ±âÈ­ÇÏ°í, ÀüÅõ¸¦ ¼³Á¤ÇÏ´Â ÄÚ·çÆ¾ ½ÇÇà
=======
    #region ì „íˆ¬ ë©”ì„œë“œ
    void BattleInit()
    {
        // ì  ìœ ë‹› ì„¤ì •
        g_EnemyUnit = GameManager.Instance.m_UnitManager.SetUnitEntityByName(GameManager.Instance.g_sEnemyBattleUnit);
        state = BattleState.START;
>>>>>>> Stashed changes
        BattleCoroutine = StartCoroutine(SetupBattle());
    }


<<<<<<< Updated upstream



    // ÇÃ·¹ÀÌ¾î ÅÏ ½ÃÀÛ Ã³¸®   ----------------------------------------------------------------------------------------------------------------------
=======
>>>>>>> Stashed changes
    private void PlayerAction()
    {
        //í”Œë ˆì´ì–´ ì•¡ì…˜
        state = BattleState.ACTION;
<<<<<<< Updated upstream
        dialogueText.text = playerUnit.m_sUnitName + "´Â ¾î¶»°Ô ÇÒ °Í ÀÎ°¡..";
    }
    #region ÇÃ·¹ÀÌ¾î Action Ã³¸®
=======
        dialogueText.text = playerUnit.m_sUnitName + "ÂŠÂ”ëŠ” ë¬´ì—‡ì„ í• ê¹Œ?";
    }
    #region Â”ÂŒí”Œë ˆì´ì–´ ì•¡ì…˜ ì²˜ë¦¬
>>>>>>> Stashed changes
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


<<<<<<< Updated upstream
    // ÀüÅõ Á¾·á Ã³¸®
    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "½Â¸®!";
=======

    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "ìŠ¹ë¦¬í–ˆë‹¤!";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;

>>>>>>> Stashed changes
    }

    void AfterLost()
    {
        state = BattleState.END;
<<<<<<< Updated upstream
        dialogueText.text = ".......´ç½ÅÀº ´«¾ÕÀÌ ±ô±ôÇØÁ³´Ù.";
=======
        dialogueText.text = "íŒ¨ë°°í–ˆë‹¤.";
        SceneManager.UnloadSceneAsync("BattleScene");
        GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
>>>>>>> Stashed changes
    }

    #endregion

<<<<<<< Updated upstream
    #region ÀüÅõ °ü·Ã ÄÚ·çÆ¾
    // ÀüÅõ ¼³Á¤À» Ã³¸®ÇÏ´Â ÄÚ·çÆ¾
    IEnumerator SetupBattle()
    {
        // ÇÃ·¹ÀÌ¾î¿Í ÀûÀÇ À¯´ÖÀ» »ı¼ºÇÏ°í ¹èÄ¡
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();
        //UIÀÇ ÀÌ¹ÌÁö·Î ½ºÇÁ¶óÀÌÆ®¸¦ ÁöÁ¤ÇÏ±â ¶§¹®¿¡ StationµéÀ» »èÁ¦Çß½À´Ï´Ù.
        //for(int i = 1; i< GameManager.Instance.m_UnitManager.g_PlayerUnits.Length;i++)
            //GameManager.Instance.m_UnitManager.g_PlayerUnits[i].transform.position = waitStation.position;
=======
    #region ì „íˆ¬ ì½”ë£¨í‹´
    IEnumerator SetupBattle()
    {
        //í”Œë ˆì´ì–´ ì²«ë²ˆì§¸ ìœ ë‹›ìœ¼ë¡œ ì„¤ì •, portrait ì´ë¯¸ì§€ ì„¤ì •
        playerUnit = GameManager.Instance.m_UnitManager.g_PlayerUnits[0].transform.GetComponent<UnitEntity>();
>>>>>>> Stashed changes
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;
        //ì  ìœ ë‹› ì„¤ì •, portrait ì´ë¯¸ì§€ ì„¤ì •
        enemyUnit = g_EnemyUnit.GetComponent<UnitEntity>();
        enemyHUD.g_imagePortrait.sprite = enemyUnit.m_spriteUnitImage;

<<<<<<< Updated upstream
        // ´ëÈ­ ÅØ½ºÆ®¿¡ ÀûÀÇ ÀÌ¸§À» Ç¥½Ã
        dialogueText.text = "¾ß»ıÀÇ " + enemyUnit.m_sUnitName + " ÀÌ(°¡) ³ªÅ¸³µ´Ù...";

        // ÇÃ·¹ÀÌ¾î¿Í ÀûÀÇ HUD¸¦ ¾÷µ¥ÀÌÆ®
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // ÀüÅõ ¼³Á¤ ÈÄ Àá½Ã ´ë±â
        yield return new WaitForSeconds(2f);

        // ÇÃ·¹ÀÌ¾î ÅÏÀ¸·Î »óÅÂ ÀüÈ¯
        PlayerAction();
    }
    #region ÇÃ·¹ÀÌ¾î Action Ã³¸®
=======
        dialogueText.text = enemyUnit.m_sUnitName + "ê°€ ë‚˜íƒ€ë‚¬ë‹¤!";

        //UI ì„¤ì •
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(2f);

        //ì•¡ì…˜ ì²˜ë¦¬ë¡œ ë„˜ì–´ê°
        PlayerAction();
    }
    #region Â”ÂŒí”Œë ˆì´ì—¬ ì•¡ì…˜ ì²˜ë¦¬ë¶€ë¶„
>>>>>>> Stashed changes
    IEnumerator PlayerTurn_Attack()
    {
        state = BattleState.PLAYERTURN;
        //°ø°İ ½ÇÇà
        playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
        enemyHUD.SetHP(enemyUnit.m_iCurrentHP);
        dialogueText.text = playerUnit.m_sUnitName + "ÀÇ " + playerUnit.GetSkillname(playerUnit,m_iPlayerActionIndex)+" °ø°İ!!";
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

<<<<<<< Updated upstream
        // ÇÃ·¹ÀÌ¾îÀÇ Ã¼·ÂÀ» HUD¿¡ ¾÷µ¥ÀÌÆ®ÇÏ°í ´ëÈ­ ÅØ½ºÆ® Ç¥½Ã
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "Ã¼·ÂÀ» 5 È¸º¹Çß´Ù!";
=======
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "ì•„ì´í…œ ì‚¬ìš©!";
>>>>>>> Stashed changes

        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    IEnumerator PlayerTurn_Change()
    {
        state = BattleState.PLAYERTURN;

<<<<<<< Updated upstream
        // ÇÃ·¹ÀÌ¾î ±³Ã¼
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];
        //playerUnit.transform.position = waitStation.position; // ÀÌÀü ÇÃ·¹ÀÌ¾î ÀÌµ¿
        //newPlayerGO.transform.position = playerBattleStation.position;
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;


        // ÇÃ·¹ÀÌ¾îÀÇ Ã¼·ÂÀ» HUD¿¡ ¾÷µ¥ÀÌÆ®
=======
        //í”Œë ˆì´ì–´ ìœ ë‹›ì—ì„œ ê°€ì ¸ì˜¨ ìœ ë‹›ìœ¼ë¡œ ì„¤ì •
        GameObject newPlayerGO = GameManager.Instance.m_UnitManager.g_PlayerUnits[m_iPlayerActionIndex];

        //ìŠ¤í¬ë¦½íŠ¸ë¥¼ ì§€ì •í•˜ê³  ì´ˆê¸°í™”
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();
        playerHUD.g_imagePortrait.sprite = playerUnit.m_spriteUnitImage;

        
>>>>>>> Stashed changes
        playerHUD.SetHUD(playerUnit);
        yield return new WaitForSeconds(2f);

        Process();
        isPlayed = true;
    }
    #endregion

<<<<<<< Updated upstream
    // ÀûÀÇ ÅÏÀ» Ã³¸®ÇÏ´Â ÄÚ·çÆ¾
=======
    //ì  ê³µê²© ì²˜ë¦¬
>>>>>>> Stashed changes
    IEnumerator EnemyTurn()
    {
        // ì „íˆ¬ ìƒíƒœ ë³€ê²½
        state = BattleState.ENEMYTURN;
<<<<<<< Updated upstream
        // ÀûÀÌ °ø°İÇÏ°í ´ëÈ­ ÅØ½ºÆ® ¾÷µ¥ÀÌÆ®
=======
        // ëœë¤ ì¸ë±ìŠ¤ë¡œ ê³µê²© ì‹¤í–‰
>>>>>>> Stashed changes
        int randomAttackIndex = Random.Range(0, 2);
        enemyUnit.AttackByIndex(enemyUnit, playerUnit, randomAttackIndex);
        //í…ìŠ¤íŠ¸ ì²˜ë¦¬
        string AttackName = enemyUnit.GetSkillname(enemyUnit,randomAttackIndex);
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
<<<<<<< Updated upstream
        dialogueText.text = enemyUnit.m_sUnitName + " ÀÇ " + AttackName + "°ø°İ!";


        // ÇÃ·¹ÀÌ¾î°¡ µ¥¹ÌÁö¸¦ ¹Ş°í Ã¼·Â ¾÷µ¥ÀÌÆ®
=======
        dialogueText.text = enemyUnit.m_sUnitName + "ì˜ " + AttackName + "ê³µê²©!";
>>>>>>> Stashed changes

        yield return new WaitForSeconds(1f);
        //ì²´ë ¥ ê²€ì‚¬ í›„ ì§„í–‰
        if (enemyUnit.m_iCurrentHP <= 0 || playerUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

    }
<<<<<<< Updated upstream
    // ÇÃ·¹ÀÌ¾î È¸º¹À» Ã³¸®ÇÏ´Â ÄÚ·çÆ¾
=======
>>>>>>> Stashed changes


    IEnumerator Result()
    {
<<<<<<< Updated upstream
        dialogueText.text = "ÅÏ ½ÇÇà ¿Ï·á..";
=======
        dialogueText.text = "Â„í„´ ì‹¤í–‰ ì™„ë£Œ";
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    #region ¹öÆ° Å¬¸¯ ÀÌº¥Æ®
    // °ø°İ ¹öÆ° Å¬¸¯ ½Ã È£ÃâµÇ´Â ¸Ş¼­µå
    public void OnButton(GameManager.Action action, int index)
    {
        // ÇÃ·¹ÀÌ¾î ÅÏÀÌ ¾Æ´Ñ °æ¿ì¿¡´Â ¾Æ¹« ÀÛ¾÷µµ ¼öÇàÇÏÁö ¾ÊÀ½
=======
    #region ë²„íŠ¼ í´ë¦­ ë©”ì„œë“œ
    public void OnButton(GameManager.Action action, int index)
    {
        //ë²„íŠ¼ ë™ì‘ì„ actionìœ¼ë¡œ ë°›ê³  ì´ì— ë”°ë¼ processì—ì„œ ì§„í–‰
>>>>>>> Stashed changes
        if (state != BattleState.ACTION)
            return;
        m_ePlayerAction = action;
        m_iPlayerActionIndex = index;
        //ë‹¤ì‹œ ê¸°ë³¸ UI í™œì„±í™”
        g_BattleButtons.SetActive(true);
        //í•˜ìœ„ ë²„íŠ¼ ì‚­ì œ
        GameObject[] destroy = GameObject.FindGameObjectsWithTag("CreatedButtons");
        for (int i = 0; i< destroy.Length;i++)
            Destroy(destroy[i]);
        
        
        

        state = BattleState.PROCESS;
        Process();
    }
    #endregion
}
