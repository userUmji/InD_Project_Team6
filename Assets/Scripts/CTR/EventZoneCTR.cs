using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZoneCTR: MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    public string[] g_gmonster_List;
    public float g_fpercent; // 몬스터 등장 확률
    public Coroutine FindCoroutine;


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(FindCoroutine);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.g_GameState == GameManager.GameState.INPROGRESS)
        {
            if (collision.CompareTag("Player"))
            {
                if (FindCoroutine == null)
                {
                    Debug.Log("adf");
               
                    FindCoroutine = StartCoroutine("Find_Monster");
                    
                }
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindCoroutine = StartCoroutine("Find_Monster");
        }
    }

    IEnumerator Find_Monster() // 특정 구역안에 몬스터와 결투시작
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            int random_percent_num = Random.Range(1, 101); // 퍼센트관련 숫자 뽑기

            if (random_percent_num <= g_fpercent) // percent이 변수 안에 들어있는 숫자 만큼의 퍼센트로 이벤트 발생
            {
                int random_monster_number = Random.Range(0, g_gmonster_List.Length); // 몬스터 뽑기
                GameManager.Instance.LoadBattleScene(g_gmonster_List[random_monster_number]);
                FindCoroutine = null;
                break;
            }
        }
    }
}
