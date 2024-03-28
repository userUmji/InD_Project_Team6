using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public Item g_Iget_Item; // 획득한 아이템

    public Slot g_Sselect_Item; // 현재 인벤토리에서 선택한 오브젝트 정보
    //public Miri_Slot select_Item_Miri; // 미리보기 인벤토리에서 선택한 오브젝트 정보
    public GameObject g_gselect_Item_Ob;

    public Item g_Iclick_Item; // 클릭한 아이템s
    public int g_iclick_Item_Count; // 클릭한 아이템 갯수

    public TextMeshProUGUI g_tmoney_View; // 현재 소지한 금액 UI
    public int g_imoney; // 소지 금액


    // Start is called before the first frame update

    private void Awake()
    {
        g_ICinstance = this;
        for (int i = 0; i < g_ginventory.transform.childCount; i++)  // 유니티 창에서 슬롯을 넣어주는게 아니고 스크립트에서 넣어주는거
        {
            g_Sslot[i] = g_ginventory.transform.GetChild(i).GetComponent<Slot>(); // 유니티상에서 인벤토리라는 오브젝트 안에 슬롯들이 있기때문에 그 슬롯들을 가져와서 배열에 넣어줌
        }
    }
    private void Start()
    {
       
        //g_gin_V.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        View_Inventory();

        g_tmoney_View.text = g_imoney.ToString();
    }

    public void Check_Slot(int num = 1) // 획득한 아이템을 인벤토리에 넣어주는 함수
    {
        Item item = null; // 획득한 아이템이 인벤토리에 있는지 없는지를 판단해주는 변수
        foreach (Slot slot_B in g_Sslot)
        {
            if (slot_B != null && slot_B.g_Ihave_item != null)  // 인벤토리가 비어있지 않다면
            {
                if (slot_B.g_Ihave_item.item_Name == g_Iget_Item.item_Name) // 현재 가지고 있는 아이템과 인벤토리에 있는 아이템이 같다면
                {
                    item = g_Iget_Item; // 변수에 현재 가지고 있는 아이템을 넣어줌
                    break;
                }
            }
        }
        Put_Invent(item, num);
    }

    public void Put_Invent(Item item, int num = 1)
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
                    if (g_Sslot[i].g_Ihave_item.item_Name == item.item_Name) // 인벤토리에 현재 가지고 있는 아이템이 있다면
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
            }
        }
        else if (g_gin_V.transform.localScale == new Vector3(0, 0, 1))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //g_gin_V.gameObject.SetActive(true); // 켜줌
                g_gin_V.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
