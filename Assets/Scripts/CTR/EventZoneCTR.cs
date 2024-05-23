using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster_Info
{
    public string MonsterName;
    public int persent;

    public Monster_Info(string monsterName, int persent_)
    {
        MonsterName = monsterName;
        persent = persent_;
    }
}
public class EventZoneCTR: MonoBehaviour
{
    public int[] g_iLevelBoundary;
    public Coroutine FindCoroutine;
    [Header("0번: 봄, 1번: 여름, 2번: 가을, 3번: 겨울")]
    public int g_iseason_Check;

    public List<Monster_Info> monsters = new List<Monster_Info>();
    public List<string> monsters_name;
    int random;
    private void Start()
    {
        Reset_List();
    }
    private void Reset_List()
    {
        // 봄
        if (g_iseason_Check == 0)
        {
            Monster_Info mon_If = new Monster_Info("일반도깨비", 100);
            monsters.Add(mon_If);
        }
        // 여름
        else if (g_iseason_Check == 1)
        {
            Monster_Info mon_If = new Monster_Info("일반도깨비", 50);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("저승차사", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("맥", 25);
            monsters.Add(mon_If2);
        }
        // 가을
        else if (g_iseason_Check == 2)
        {
            Monster_Info mon_If = new Monster_Info("일반도깨비", 25);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("불가사리", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("강철이", 25);
            monsters.Add(mon_If2);

            Monster_Info mon_If3 = new Monster_Info("맥", 15);
            monsters.Add(mon_If3);

            Monster_Info mon_If4 = new Monster_Info("저승차사", 10);
            monsters.Add(mon_If4);
        }
        // 겨울
        else if (g_iseason_Check == 3)
        {
            Monster_Info mon_If = new Monster_Info("일반도깨비", 25);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("장산범", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("백요호", 25);
            monsters.Add(mon_If2);

            Monster_Info mon_If3 = new Monster_Info("불가사리", 15);
            monsters.Add(mon_If3);

            Monster_Info mon_If4 = new Monster_Info("강철이", 10);
            monsters.Add(mon_If4);
        }
    }
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
            
            if (random_percent_num <= 10)
            {

                for(int i = 0;i<= monsters.Count -1; i++)
                {
                    if (monsters[i].persent == 10)
                    {
                        monsters_name.Add( monsters[i].MonsterName);
                    }
                }
            }
            else if (random_percent_num <= 15)
            {

                for (int i = 0; i <= monsters.Count-1; i++)
                {
                    if (monsters[i].persent == 15)
                    {
                        monsters_name.Add(monsters[i].MonsterName);
                    }
                }
            }
            else if (random_percent_num <= 25)
            {

                for (int i = 0; i <= monsters.Count-1; i++)
                {
                    if (monsters[i].persent == 25)
                    {
                        monsters_name.Add(monsters[i].MonsterName);
                    }
                }
            }
            else if (random_percent_num <= 50)
            {

                for (int i = 0; i <= monsters.Count-1; i++)
                {
                    if (monsters[i].persent == 50)
                    {
                        monsters_name.Add(monsters[i].MonsterName);
                    }
                }
            }
            else if (random_percent_num <= 100)
            {
                

                for (int i = 0; i <= monsters.Count-1; i++)
                {
                    if (monsters[i].persent == 100)
                    {
                        monsters_name.Add(monsters[i].MonsterName);
                    }
                }
            }

            if(monsters_name.Count != 0)
            {
                random = Random.Range(0, monsters_name.Count);
                print(monsters_name[random] + "등장!!!!!!!!!!!!!!!!!!!!!!!");
                GameManager.Instance.LoadBattleScene(monsters_name[random]);
                FindCoroutine = null;
                monsters_name.Clear();
                break;
            }


            /* for (int i =0; i < g_fpercent.Length; i++)
             {
                 if (g_fpercent[i] <= random_percent_num) // percent이 변수 안에 들어있는 숫자 만큼의 퍼센트로 이벤트 발생
                 {
                     int random_monster_number = Random.Range(0, g_gmonster_List.Length); // 몬스터 뽑기
                     GameManager.Instance.LoadBattleScene(g_gmonster_List[i]);
                     FindCoroutine = null;
                     break;
                 }
             }  */
        }
    }
}
