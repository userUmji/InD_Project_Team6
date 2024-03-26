using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    public GameObject[] g_gmonster_List;
    public GameObject g_gconfirmed_Monster; // 등장한 몬스터
    public float g_fpercent; // 몬스터 등장 확률

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine("Find_Monster");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("Find_Monster");
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
                g_gconfirmed_Monster = g_gmonster_List[random_monster_number]; // 랜덤으로 뽑은 몬스터를 현재 등장하는 몬스터에 넣어줌.
                print("야생의 " + g_gmonster_List[random_monster_number].name + "등장했다!");
                break;
            }
        }
    }
}
