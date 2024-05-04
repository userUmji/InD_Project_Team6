using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
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
            Temp_Action();
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hit.collider.tag == "Item")
                {
                    // 아이템 처리 코드
                    Inventory_Controller.g_ICinstance.Set_GetItem(hit.transform.gameObject);
                    Inventory_Controller.g_ICinstance.Check_Slot();
                    Destroy(hit.transform.gameObject);
                    AudioManager._instance.PlaySfx(AudioManager.Sfx.Item);
                }
                else if (hit.collider.tag == "object")
                {
                    // 오브젝트 처리 코드
                    _instance.Act(m_scanObject);
                    AudioManager._instance.PlaySfx(AudioManager.Sfx.Talk);
                }
            }
            if (hit.collider.tag == "Event")
            {
                if (!m_scanObject.GetComponent<ObjData>().isNpc)
                {
                    if (!m_scanObject.GetComponent<ObjData>().isInteracted)
                    {
                        // 스캔된 오브젝트가 이전에 상호작용되지 않았을 경우에만 아래의 코드를 실행합니다.
                        m_scanObject.GetComponent<ObjData>().isInteracted = true;
                        _instance.Act(m_scanObject);
                        AudioManager._instance.PlaySfx(AudioManager.Sfx.Talk);
                    }
                    else
                    {
                        // 스페이스바를 눌렀을 때 한 번 더 상호작용하도록 설정합니다.
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            _instance.Act(m_scanObject);
                            AudioManager._instance.PlaySfx(AudioManager.Sfx.Talk);
                        }
                    }
                }
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
        animator.SetFloat("InputX", m_fx);
        animator.SetFloat("InputY", m_fy);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // animations // 0 -> 앞, 1 -> 뒤, 3 -> 옆
            // animator.SetBool("Walk", false);
            //Swith_Motion

            Run();
        }
        else
        {
          //  animator.SetBool("Run", false);
            Walk();
        }
        
    }

    void Walk()
    {
        if (m_fx == 0 && m_fy == 0)
        {
          //  animator.SetBool("Walk", false);
        }
        else
        {
         //   animator.SetBool("Walk", true);
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
           // animator.SetBool("Run", false);
        }
        else
        {
          //  animator.SetBool("Run", true);
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

    private void Temp_Action()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.SaveALLPlayerUnit();
        }
    }
}
