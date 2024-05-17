using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Conroller : MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    private Vector2 m_vmovePos = Vector2.zero;
    public Animator m_aanimator;

    public float g_fre_Move;
    public float g_fspeed;

    // Start is called before the first frame update
    void Start()
    {
        m_aanimator = GetComponent<Animator>();
        Location(true);
    }

    // Update is called once per frame
    void Update()
    {
       // if(GameManager.Instance.g_GameState == GameManager.GameState.INPROGRESS)
            E_Move();
    }
   

    public void Location(bool checek) // 이동할 위치를 선정해줌
    {
        if(checek == true)
        {
            m_aanimator.SetBool("Move", true);
            float x = Random.Range(transform.position.x + -3f, transform.position.x + 3f);
            float y = Random.Range(transform.position.y + -3f, transform.position.y + 3f);

            m_vmovePos = new Vector2(x, y);
        }
    }


    public void E_Move() // 적 이동
    {
        if(new Vector2(transform.position.x, transform.position.y) == m_vmovePos)
        {
            m_aanimator.SetBool("Move", false);
            g_fre_Move -= Time.deltaTime;
            if(g_fre_Move <= 0)
            {
                Location(true);
                g_fre_Move = Random.Range(0, 3f);
            }
        }

        if (transform.position.x < m_vmovePos.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x > m_vmovePos.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position = Vector2.MoveTowards(transform.position, m_vmovePos, g_fspeed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))
        {
            m_vmovePos = collision.transform.position;
            Location(false);
        }
    }
}
