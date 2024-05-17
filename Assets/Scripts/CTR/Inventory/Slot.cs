using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    public static Slot g_SInstance;
    public Image g_iitem_Image;
    public Sprite g_snull_item_Image;
    public ItemEntity g_Ihave_item;
    public TextMeshProUGUI g_tdescription_Window; // 설명창
    public TextMeshProUGUI g_titem_Num_Name; // 아이템 이름, 개수
    public Image g_iitem_Img_View; // 아이템 이미지
    public int g_iitem_Number; // 획득한 아이템 갯수
    Slot_Button s_B;

    private void Awake()
    {
        g_SInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        s_B = GetComponent<Slot_Button>();
    }

    // Update is called once per frame
    void Update()
    {
        Show_Item();
        Empty_Check();
    }

    public void OnMouseEnter() // UI가 마우스 감지
    {
        if(g_iitem_Number != 0 && Inventory_Controller.g_ICinstance.lock_UI)
        {
            g_iitem_Img_View.gameObject.SetActive(true);
            //  Inventory_Controller.g_ICinstance.g_Iclick_Item = g_Ihave_item;
            Inventory_Controller.g_ICinstance.g_Sclick_Slot = this;

            g_tdescription_Window.text = g_Ihave_item.m_sItemDescription;
            g_titem_Num_Name.text = g_Ihave_item.m_sItemName + " X " + g_iitem_Number.ToString();
            g_iitem_Img_View.sprite = g_Ihave_item.m_ItemSprite;
        }
    }

    public void Refresh()
    {
        if(g_iitem_Number != 0)
        {
            g_titem_Num_Name.text = g_Ihave_item.m_sItemName + " X " + g_iitem_Number.ToString();
        }
        
    }

    public void Empty_Check()
    {
       // g_titem_Num_Name.text = item_Number.ToString(); // 소지하고 있는 아이템 갯수
        if (g_Ihave_item != null)
        {
            if (g_iitem_Number == 0) // 아이템이 없다면 아이템을 다 사용했다면
            {
                g_Ihave_item = null;
                g_tdescription_Window.text = " ";
                g_titem_Num_Name.text = " ";
                g_iitem_Img_View.gameObject.SetActive(false);
                g_iitem_Image.gameObject.SetActive(false);
                // 정보 
                /* g_iitem_Image.sprite = g_snull_item_Image;       // 초기
                 g_titem_Number_UI.text = " ";     // 화*/
            }
            else
            {
                g_iitem_Image.gameObject.SetActive(true);
                g_iitem_Image.sprite = g_Ihave_item.m_ItemSprite;
            }
        }
        else
        {
            //  g_titem_Number_UI.text = " ";
            g_iitem_Image.gameObject.SetActive(false);
            g_iitem_Number = 0;
        }
    }
    public void Input_Item(ItemEntity item, int num = 1) // 아이템 넣기
    {
        if (item != null) // 받아온 아이템이 있다면
        {
            g_Ihave_item = item;  // 가진 아이템 정보에 가져온 아이템을 넣고
            g_iitem_Number += num;
        }
    }

    void Show_Item()
    {
        if (g_Ihave_item != null)
        {
            g_iitem_Image.sprite = g_Ihave_item.m_ItemSprite; // 아이템 이미지 할당
        }
        else
        {
            g_iitem_Image.sprite = g_snull_item_Image;
        }
    }
}
