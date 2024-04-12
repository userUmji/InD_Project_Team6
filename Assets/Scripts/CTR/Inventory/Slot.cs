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
    public Item g_Ihave_item;

    public int item_Number; // 획득한 아이템 갯수
    Slot_Button s_B;

    [Header("UI할당")]
    public Image item_View_Img;
    public TextMeshProUGUI item_Number_Text;
    public TextMeshProUGUI item_Description_Text;
    public TextMeshProUGUI item_Name_Text;

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

    public void Empty_Check()
    {
        if (g_Ihave_item != null)
        {
            if (item_Number <= 0) // 아이템이 없다면 아이템을 다 사용했다면
            {
                g_Ihave_item = null;               // 정보 
                g_iitem_Image.gameObject.SetActive(false);
            }
            else
            {
                g_iitem_Image.gameObject.SetActive(true);
                g_iitem_Image.sprite = g_Ihave_item.item_Image;
                
            }
        }
        else
        {
            if (item_Number <= 0) // 아이템이 없다면 아이템을 다 사용했다면
            {
                g_Ihave_item = null;               // 정보 
                g_iitem_Image.gameObject.SetActive(false);
            }
            item_Number = 0;
        }
    }

    public void OnPointerEnter()
    {
        print(gameObject.name);
      //  item_View_Img.sprite = g_Ihave_item.item_Image;
        //item_Number_Text.text = item_Number.ToString();
        //item_Description_Text = g_Ihave_item.
       // item_Name_Text.text = g_Ihave_item.item_Name + " : " + item_Number.ToString();
    }



    public void Input_Item(Item item, int num = 1) // 아이템 넣기
    {
        if (item != null) // 받아온 아이템이 있다면
        {
            g_Ihave_item = item;  // 가진 아이템 정보에 가져온 아이템을 넣고
            item_Number += num;
        }
    }

    void Show_Item()
    {
        if (g_Ihave_item != null)
        {
            g_iitem_Image.sprite = g_Ihave_item.item_Image ; // 아이템 이미지 할당
        }
        else
        {
            g_iitem_Image.sprite = g_snull_item_Image;
        }
    }
}
