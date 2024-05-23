using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZoneCTR: MonoBehaviour
{
    [System.Serializable]
    public class Monster
    {
        public string m_sName;
        public int m_iChance;
    }
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    [SerializeField] public Monster[] g_gmonster_List;
    public float g_fpercent; // 몬스터 등장 확률
    public int[] g_iLevelBoundary;
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
                int max_chace = 0;
                for (int i = 0; i< g_gmonster_List.Length;i++)
                {
                    max_chace += g_gmonster_List[i].m_iChance;
                }
                int random_monster_number = Random.Range(0, max_chace); // 몬스터 뽑기\
                string name = CalMonsterChance(g_gmonster_List, random_monster_number);

                int random_monster_lvl = Random.Range(g_iLevelBoundary[0], g_iLevelBoundary[1]);
                GameManager.Instance.LoadBattleScene(g_gmonster_List[random_monster_number].m_sName,random_monster_lvl);

                FindCoroutine = null;
                break;
            }
        }
    }

    private string CalMonsterChance(Monster[] mons, int chance)
    {
        int[] chanceArr = new int[mons.Length];
        for (int i = 0; i < mons.Length; i++)
        {
            chanceArr[i] = 0;
            for (int j = 0; j < i +1; j++)
            {
                chanceArr[i] += mons[j].m_iChance;
            }
        }
        for (int i = 0; i < chanceArr.Length; i++)
        {
            if(i == 0)
            {
                if (chanceArr[i] > chance)
                    return mons[i].m_sName;
            }
            if (chanceArr[i-1] < chance && chance < chanceArr[i])
            {
                return mons[i].m_sName;
            }
        }
        return "더미";
    }
}
