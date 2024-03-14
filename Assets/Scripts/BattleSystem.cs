using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 전투 상태를 정의하는 열거형
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WiN, LOST }

public class BattleSystem : MonoBehaviour
{
    // 플레이어와 적의 프리팹
    public GameObject[] playerPrefabs;
    public GameObject enemyPrefab;

    // 플레이어와 적이 전투하는 위치
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    // 현재 플레이어와 적의 유닛
    Unit playerUnit;
    Unit enemyUnit;

    // 전투 중 발생하는 대화를 표시하는 UI 텍스트
    public Text dialogueText;

    // 플레이어와 적의 HUD(Head-Up Display)를 관리하는 객체
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    // 전투 상태
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        // 전투 시작 상태로 초기화하고, 전투를 설정하는 코루틴 실행
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // 전투 설정을 처리하는 코루틴
    IEnumerator SetupBattle()
    {
        // 플레이어와 적의 유닛을 생성하고 배치
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        GameObject playerGO = Instantiate(playerPrefabs[randomPlayerIndex], playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        // 대화 텍스트에 적의 이름을 표시
        dialogueText.text = "야생의 " + enemyUnit.unitName + " 이(가) 나타났다...";

        // 플레이어와 적의 HUD를 업데이트
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // 전투 설정 후 잠시 대기
        yield return new WaitForSeconds(2f);

        // 플레이어 턴으로 상태 전환
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // 플레이어 공격을 처리하는 코루틴  --------------------------------------------------------------------------------------------------------
    IEnumerator PlayerAttack()
    {
        // 적에게 데미지를 입히고 결과 받아옴
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        // 적의 체력을 HUD에 업데이트
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + "의 공격!!";

        // 일정 시간 대기
        yield return new WaitForSeconds(1f);

        // 적이 죽었는지 확인하고 상태 전환
        if (isDead)
        {
            state = BattleState.WiN;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    // 적의 턴을 처리하는 코루틴
    IEnumerator EnemyTurn()
    {
        // 적이 공격하고 대화 텍스트 업데이트
        dialogueText.text = enemyUnit.unitName + " 의 공격!";

        yield return new WaitForSeconds(1f);

        // 플레이어가 데미지를 받고 체력 업데이트
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        // 플레이어가 죽었는지 확인하고 상태 전환
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    // 전투 종료 처리
    void EndBattle()
    {
        // 전투 결과에 따라 대화 텍스트 업데이트
        if (state == BattleState.WiN)
        {
            dialogueText.text = "승리!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = ".......당신은 눈앞이 깜깜해졌다.";
        }
    }

    // 플레이어 턴 시작 처리   ----------------------------------------------------------------------------------------------------------------------
    void PlayerTurn()
    {
        dialogueText.text = playerUnit.unitName + "는 어떻게 할 것 인가..";
    }


    // 공격 버튼 클릭 시 호출되는 메서드
    public void OnAttackButton()
    {
        // 플레이어 턴이 아닌 경우에는 아무 작업도 수행하지 않음
        if (state != BattleState.PLAYERTURN)
            return;

        // 플레이어 공격 코루틴 실행
        StartCoroutine(PlayerAttack());
    }

    // 회복 버튼 클릭 시 호출되는 메서드
    public void OnHealButton()
    {
        // 플레이어 턴이 아닌 경우에는 아무 작업도 수행하지 않음
        if (state != BattleState.PLAYERTURN)
            return;

        // 플레이어 회복 코루틴 실행
        StartCoroutine(PlayerHeal());
    }


    // 플레이어 교체 버튼 클릭 시 호출되는 메서드
    public void OnChangePlayerButton()
    {
        // 플레이어 턴이 아닌 경우에는 아무 작업도 수행하지 않음
        if (state != BattleState.PLAYERTURN)
            return;

        // 플레이어 교체
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        GameObject newPlayerGO = Instantiate(playerPrefabs[randomPlayerIndex], playerBattleStation);
        Destroy(playerUnit.gameObject); // 이전 플레이어 삭제
        playerUnit = newPlayerGO.GetComponent<Unit>();

        // 플레이어의 체력을 HUD에 업데이트
        playerHUD.SetHUD(playerUnit);

        // 적 턴으로 전환
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }



    // 플레이어 회복을 처리하는 코루틴
    IEnumerator PlayerHeal()
    {
        // 플레이어 체력 회복
        playerUnit.Heal(5);

        // 플레이어의 체력을 HUD에 업데이트하고 대화 텍스트 표시
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        // 적의 턴으로 상태 전환
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    

    
}
