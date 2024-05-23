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
    [System.Serializable]
    public class Monster
    {
        public string m_sName;
        public int m_iChance;
    }
    // g_fCharacterSpeed -> g�� �۷ι�(public) m�� ���(private) ���� f(float)/i(int)/s(string)
    [SerializeField] public Monster[] g_gmonster_List;
    public float g_fpercent; // ���� ���� Ȯ��
    public int[] g_iLevelBoundary;
    public Coroutine FindCoroutine;
    [Header("0��: ��, 1��: ����, 2��: ����, 3��: �ܿ�")]
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
        // ��
        if (g_iseason_Check == 0)
        {
            Monster_Info mon_If = new Monster_Info("�Ϲݵ�����", 100);
            monsters.Add(mon_If);
        }
        // ����
        else if (g_iseason_Check == 1)
        {
            Monster_Info mon_If = new Monster_Info("�Ϲݵ�����", 50);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("��������", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("��", 25);
            monsters.Add(mon_If2);
        }
        // ����
        else if (g_iseason_Check == 2)
        {
            Monster_Info mon_If = new Monster_Info("�Ϲݵ�����", 25);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("�Ұ��縮", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("��ö��", 25);
            monsters.Add(mon_If2);

            Monster_Info mon_If3 = new Monster_Info("��", 15);
            monsters.Add(mon_If3);

            Monster_Info mon_If4 = new Monster_Info("��������", 10);
            monsters.Add(mon_If4);
        }
        // �ܿ�
        else if (g_iseason_Check == 3)
        {
            Monster_Info mon_If = new Monster_Info("�Ϲݵ�����", 25);
            monsters.Add(mon_If);

            Monster_Info mon_If1 = new Monster_Info("����", 25);
            monsters.Add(mon_If1);

            Monster_Info mon_If2 = new Monster_Info("���ȣ", 25);
            monsters.Add(mon_If2);

            Monster_Info mon_If3 = new Monster_Info("�Ұ��縮", 15);
            monsters.Add(mon_If3);

            Monster_Info mon_If4 = new Monster_Info("��ö��", 10);
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

    IEnumerator Find_Monster() // Ư�� �����ȿ� ���Ϳ� ��������
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            int random_percent_num = Random.Range(1, 101); // �ۼ�Ʈ���� ���� �̱�
            
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
                print(monsters_name[random] + "����!!!!!!!!!!!!!!!!!!!!!!!");
                GameManager.Instance.LoadBattleScene(monsters_name[random]);
                int max_chace = 0;
                for (int i = 0; i< g_gmonster_List.Length;i++)
                {
                    max_chace += g_gmonster_List[i].m_iChance;
                }
                int random_monster_number = Random.Range(0, max_chace); // ���� �̱�\
                string name = CalMonsterChance(g_gmonster_List, random_monster_number);

                int random_monster_lvl = Random.Range(g_iLevelBoundary[0], g_iLevelBoundary[1]);
                GameManager.Instance.LoadBattleScene(g_gmonster_List[random_monster_number].m_sName,random_monster_lvl);

                FindCoroutine = null;
                monsters_name.Clear();
                break;
            }


            /* for (int i =0; i < g_fpercent.Length; i++)
             {
                 if (g_fpercent[i] <= random_percent_num) // percent�� ���� �ȿ� ����ִ� ���� ��ŭ�� �ۼ�Ʈ�� �̺�Ʈ �߻�
                 {
                     int random_monster_number = Random.Range(0, g_gmonster_List.Length); // ���� �̱�
                     GameManager.Instance.LoadBattleScene(g_gmonster_List[i]);
                     FindCoroutine = null;
                     break;
                 }
             }  */
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
        return "����";
    }
}
