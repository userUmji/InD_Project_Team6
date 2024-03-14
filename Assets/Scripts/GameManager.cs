using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Unit
{
    // 게임 상태를 나타내는 열거형
    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    // 현재 게임 상태
    public State state;

    // 적 생존 여부
    public bool isLive;

    // 게임 시작 시 호출되는 Awake 함수
    void Awake()
    {
        // 전투 시작 알림
        state = State.start;
        BattleStart();
    }

    // 전투 시작 시 호출되는 함수
    void BattleStart()
    {
        // 전투 시작 시 캐릭터 등장 애니메이션 및 효과 추가
        // 플레이어와 적에게 턴 넘기기
        state = State.playerTurn;
    }

    // 플레이어 공격 버튼 클릭 시 호출되는 함수
    public void PlayerAttackButton()
    {
        // 플레이어 턴이 아닐 때는 함수 종료
        if (state != State.playerTurn)
        {
            return;
        }

        // 플레이어 공격 코루틴 시작
        StartCoroutine(PlayerAttack());
    }

    // 플레이어 공격 코루틴
    IEnumerator PlayerAttack()
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);
        Debug.Log("플레이어 공격");

        // 공격 스킬, 데미지 등의 코드 작성
        // 적이 죽으면 전투 종료
        if (!isLive)
        {
            state = State.win;
            EndBattle();
        }
        // 적이 살아있으면 턴을 적군에게 전환
        else
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    // 전투 종료 시 호출되는 함수
    void EndBattle()
    {
        Debug.Log("전투 종료");
    }

    // 적군 턴 코루틴
    IEnumerator EnemyTurn()
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 적 공격 코드 작성
        // 적 공격 종료 후 플레이어 턴으로 전환
        state = State.playerTurn;
    }
}