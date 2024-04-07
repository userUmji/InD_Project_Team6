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
    private Rigidbody2D m_Rigidbody2D;
    private GameObject m_scanObject;

    public float g_fspeed;
    public float g_frun_Speed;
    public LayerMask g_llayer;
    public TextManager _instance;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.g_GameState == GameManager.GameState.INPROGRESS)
        {
            Movement();
            Object_Interaction();
        }
    }

    private void Object_Interaction()
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
            m_scanObject = hit.collider.gameObject; // 충돌한 객체를 스캔된 객체로 설정

            if (Input.GetKeyDown(KeyCode.F)) // F 키가 눌렸는지 확인
            {
                // 충돌한 객체로부터 아이템을 가져와 인벤토리 컨트롤러의 현재 아이템으로 설정
                Inventory_Controller.g_ICinstance.g_Iget_Item = hit.transform.GetComponent<Drop_Item>().g_Iitem;
                // 인벤토리 슬롯을 확인
                Inventory_Controller.g_ICinstance.Check_Slot();
                // 충돌한 객체를 파괴
                Destroy(hit.transform.gameObject);
            }
            else if (Input.GetButtonDown("Jump") && m_scanObject != null) // 점프(스페이스바) 버튼이 눌렸고 스캔된 객체가 있는지 확인
            {
                // _instance 객체의 Act 메서드를 호출하고 스캔된 객체를 전달
                _instance.Act(m_scanObject);
            }
        }
        else // 충돌체가 없는 경우
        {
            m_scanObject = null; // 스캔된 객체를 null로 재설정
        }

    }

    private void Movement()
    {
        m_fx = _instance.isAct ? 0 : Input.GetAxisRaw("Horizontal");
        m_fy = _instance.isAct ? 0 : Input.GetAxisRaw("Vertical");

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
        Vector2 movement = new Vector2(m_fx, m_fy) * g_fspeed * Time.deltaTime;
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement); // 플레이어와 오브젝트 충돌시 떨림현상 방지 
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

        Vector2 movement = new Vector2(m_fx, m_fy).normalized * g_frun_Speed * Time.deltaTime;
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement); // 플레이어와 오브젝트 충돌시 떨림현상 방지 
    }
}
