using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글턴 인스턴스 존재를 확인하는 용도
    private static GameManager _instance;
    public UnitTable m_AssetUnitTable;
    DataAssetManager m_DataManager;


    // 게임 상태를 나타내는 열거형
    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    // 현재 게임 상태
    public State state;

    // 적 생존 여부
    public bool isLive;

    //싱글턴 구현
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //지정된 인스턴스가 없다면 지정, 이미 존재한다면 대체
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //데이터메니저 Init을 실행시킴
        m_DataManager = new DataAssetManager();
        m_DataManager.Init(m_AssetUnitTable);

        state = State.start;
        BattleStart();
    }

    //유닛 데이터를 가져오는 기능
    public UnitTable.UnitStats GetUnitData(string className)
    {
        return m_DataManager.GetUnitData(className);
    }


    #region 전투 함수
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


    // 전투 종료 시 호출되는 함수
    void EndBattle()
    {
        Debug.Log("전투 종료");
    }


    #endregion
    #region 전투 코루틴

    // 적군 턴 코루틴
    IEnumerator EnemyTurn()
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 적 공격 코드 작성
        // 적 공격 종료 후 플레이어 턴으로 전환
        state = State.playerTurn;
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

    #endregion
}