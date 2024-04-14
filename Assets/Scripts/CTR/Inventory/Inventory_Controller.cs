using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory_Controller : MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    [Header("인벤토리 온오프를 위한 부모 변수 받아옴")]
    public GameObject g_gin_V;
    [Header("실제 인벤토리")]
    public GameObject g_ginventory;
    public static Inventory_Controller g_ICinstance;
    public Slot[] g_Sslot; // 인벤토리 안에 있는 각 슬롯들
    public Slot g_Sclick_Slot; // 버릴 슬롯
    public ItemEntity g_Iget_Item; // 획득한 아이템

    public Slot g_Sselect_Item; // 현재 인벤토리에서 선택한 오브젝트 정보
    //public Miri_Slot select_Item_Miri; // 미리보기 인벤토리에서 선택한 오브젝트 정보
    public GameObject g_gselect_Item_Ob;

    public ItemEntity g_Iclick_Item; // 클릭한 아이템s
    public int g_iclick_Item_Count; // 클릭한 아이템 갯수

    public bool invent_On_Off_Check; // 인벤토리가 켜져있는지 꺼져있는지 확인해주는 변수
    public bool lock_UI; // 특수한 UI가 켜져있을때 다른것 건들지 못하게 함
    public GameObject discard_value_View; // 버릴 개수 입력 창 띄우기

    [Header("인벤토리 오른쪽 표시 UI")]
    public Image Img_View;
    public TextMeshProUGUI name_View;
    public TextMeshProUGUI Des_View;
    // Start is called before the first frame update

    private void Awake()
    {
        g_ICinstance = this;
        
    }
    private void Start()
    {
        lock_UI = true;
        g_Sslot = new Slot[g_ginventory.transform.childCount];
        for (int i = 0; i < g_ginventory.transform.childCount; i++)  // 유니티 창에서 슬롯을 넣어주는게 아니고 스크립트에서 넣어주는거
        {
            g_Sslot[i] = g_ginventory.transform.GetChild(i).GetComponent<Slot>(); // 유니티상에서 인벤토리라는 오브젝트 안에 슬롯들이 있기때문에 그 슬롯들을 가져와서 배열에 넣어줌
        }
    }
    // Update is called once per frame
    void Update()
    {
        View_Inventory();
        if (invent_On_Off_Check)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (discard_value_View.gameObject.activeSelf == false)
                {
                    discard_value_View.gameObject.SetActive(true);
                    lock_UI = false;
                }
                else if(discard_value_View.gameObject.activeSelf == true)
                {
                    discard_value_View.gameObject.SetActive(false);
                    lock_UI = true;
                }
            }
        }
    }

/*    public void Throw_Item() // 인벤토리에서 클릭한 아이템 삭제
    {
        if (g_ICinstance.g_Iclick_Item != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {

            }
            g_ICinstance.g_Iclick_Item = null;
            g_ICinstance.g_iclick_Item_Count = 0;
        }
    }*/
    public void Check_Slot(int num = 1) // 획득한 아이템을 인벤토리에 넣어주는 함수
    {
        ItemEntity item = null; // 획득한 아이템이 인벤토리에 있는지 없는지를 판단해주는 변수
        foreach (Slot slot_B in g_Sslot)
        {
            if (slot_B != null && slot_B.g_Ihave_item != null)  // 인벤토리가 비어있지 않다면
            {
                if (slot_B.g_Ihave_item.m_sItemName == g_Iget_Item.m_sItemName) // 현재 가지고 있는 아이템과 인벤토리에 있는 아이템이 같다면
                {
                    item = g_Iget_Item; // 변수에 현재 가지고 있는 아이템을 넣어줌
                    break;
                }
            }
        }
        Put_Invent(item, num);
    }

    public void Put_Invent(ItemEntity item, int num = 1)
    {
        for (int i = 0; i <= g_Sslot.Length; i++)
        {
            if (item == null) //인벤토리에 현재 가지고 있는 아이템이 없다면
            {
                if (g_Sslot[i].g_Ihave_item == null) // 인벤토리가 비어있을때
                {
                    g_Sslot[i].Input_Item(g_Iget_Item, num); // 비어있는 슬롯에 현재 아이템을 넣어줌
                    break;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (g_Sslot[i].g_Ihave_item != null) // 인벤토리가 비어있지 않다면
                {
                    if (g_Sslot[i].g_Ihave_item.m_sItemName == item.m_sItemName) // 인벤토리에 현재 가지고 있는 아이템이 있다면
                    {
                        g_Sslot[i].Input_Item(g_Iget_Item, num); // 슬롯에 아이템을 넣어줌
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }
    public void View_Inventory() // 인벤토리 온 오프 함수
    {
        if (g_gin_V.transform.localScale == new Vector3(1, 1, 1)) // 인벤토리가 켜있으면
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
            {
                for (int i = 0; i < g_ginventory.transform.childCount; i++)  // 유니티 창에서 슬롯을 넣어주는게 아니고 스크립트에서 넣어주는거
                {
                    g_Sslot[i].GetComponent<Slot_Button>().Off_Inven();
                }
                
                g_gin_V.transform.localScale = new Vector3(0, 0, 1); // 꺼줌
                invent_On_Off_Check = false;
            }
        }
        else if (g_gin_V.transform.localScale == new Vector3(0, 0, 1))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Img_View.gameObject.SetActive(false);
                name_View.text = " ";
                Des_View.text = " ";
                invent_On_Off_Check = true;
                g_gin_V.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void Set_GetItem(GameObject itemEntity)
    {
        GameObject entity = Instantiate(itemEntity,GameObject.Find("Inventory_GOs").transform);
        Destroy(entity.transform.GetComponent<SpriteRenderer>());
        Destroy(entity.transform.GetComponent<Collider2D>());
        g_Iget_Item = entity.GetComponent<ItemEntity>();
    }
}
