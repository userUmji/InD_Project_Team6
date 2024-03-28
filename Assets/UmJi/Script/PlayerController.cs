using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string) 
    private Animator animator;
    private float m_fx;
    private float m_fy;

    public float g_fspeed;
    public float g_frun_Speed;
    public LayerMask g_llayer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Drop_Item();
    }

    private void Drop_Item()
    {
        // 캐릭터가 바라보는 반대 방향으로 레이캐스트
        Vector2 lookDirection;

        // 캐릭터가 왼쪽을 바라보는 경우
        if (transform.localScale.x < 0)
        {
            lookDirection = -transform.right;
        }
        else
        {
            lookDirection = transform.right; // 초기값은 오른쪽으로 설정
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, 2, g_llayer);

        
        // 레이가 어떤 오브젝트와 충돌했는지 확인
        if (hit.collider != null)
        {
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                Inventory_Controller.g_ICinstance.g_Iget_Item = hit.transform.GetComponent<Drop_Item>().g_Iitem;

                Inventory_Controller.g_ICinstance.Check_Slot();

                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
        }
    }

    
    private void Movement()
    {
        m_fx = Input.GetAxisRaw("Horizontal");
        m_fy = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Walk", false);
            Run();
        }
        else
        {
            animator.SetBool("Run", false);
            Walk();
        }
        
    }

    void Walk()
    {
        if (m_fx == 0 && m_fy == 0)
        {
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Walk", true);
        }

        if (m_fx == -1 || m_fy == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (m_fx == 1 || m_fy == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        transform.Translate(new Vector2(m_fx, m_fy) * g_fspeed * Time.deltaTime);
    }

    void Run()
    {
        if (m_fx == 0 && m_fy == 0)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Run", true);
        }

        if (m_fx == -1 || m_fy == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (m_fx == 1 || m_fy == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.Translate(new Vector2(m_fx, m_fy).normalized * g_frun_Speed * Time.deltaTime);
    }
}
