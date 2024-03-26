using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot_Button : MonoBehaviour, IPointerClickHandler
{
    // g_fCharacterSpeed -> g는 글로벌(public) m은 멤버(private) 뒤의 f(float)/i(int)/s(string)
    public static Slot_Button g_SBinstance;
    public Image g_ishow_Item_Image; // 마우스를 따라다닐 이미지
    [SerializeField] Slot m_Sslot;
    [SerializeField] Item m_ISr_S;

    private int m_inum;

    public Canvas canvas;
    public void Awake()
    {
        g_SBinstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Sslot = GetComponent<Slot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 아이템 이동
        {
            m_ISr_S = m_Sslot.g_Ihave_item;
            m_inum = m_Sslot.item_Number;
            if (m_Sslot.g_Ihave_item != null && Inventory_Controller.g_ICinstance.g_Iclick_Item == null)// 선택된 오브젝트가 없을때
            {
                Inventory_Controller.g_ICinstance.g_Iclick_Item = m_Sslot.g_Ihave_item; // 현재 클릭한 아이템 정보를 인벤토리 스크립트 변수에 할당
                Inventory_Controller.g_ICinstance.g_iclick_Item_Count = m_Sslot.item_Number; // 현재 클릭한 아이템 정보를 인벤토리  스크립트 변수에 할당
                m_Sslot.g_Ihave_item = null; // 누른 슬롯에 아이템을 비워줌
                m_Sslot.item_Number = 0; // 누른 슬롯에 아이템 숫자를 지워줌
            }

            else if (Inventory_Controller.g_ICinstance.g_Iclick_Item != null && m_Sslot.g_Ihave_item == null)// 아이템을 선택한 상태에서 빈 슬롯을 클릭했을 때
            {
                m_Sslot.g_Ihave_item = Inventory_Controller.g_ICinstance.g_Iclick_Item; // 클릭한 슬롯 아이템에 클릭했었던 아이템 정보를 할당
                m_Sslot.item_Number = Inventory_Controller.g_ICinstance.g_iclick_Item_Count; // 클릭한 슬롯 아이템 갯수에 클릭했던 아이템의 아이템 갯수를 할당
                Inventory_Controller.g_ICinstance.g_Iclick_Item = null; // 클릭했던 아이템 정보 삭제
                Inventory_Controller.g_ICinstance.g_iclick_Item_Count = 0; // 클릭했던 아이템 갯수 삭제
            }
            else if (Inventory_Controller.g_ICinstance.g_Iclick_Item != null && m_Sslot.g_Ihave_item != null) // 아이템을 선택한 상태에서 비어있지 않은 슬롯을 클릭했을 때
            {
                Item temp = Inventory_Controller.g_ICinstance.g_Iclick_Item; // 클릭했던 아이템 정보를 임시 변수에 할당
                Inventory_Controller.g_ICinstance.g_Iclick_Item = m_Sslot.g_Ihave_item; // 클릭했던 아이템 정보에 현재 클릭한 아이템 정보 할당
                m_Sslot.g_Ihave_item = temp; // 현재 클릭한 슬롯에 클릭했던 아이템 정보를 가지고 있는 임시변수값 할당

                int temp_count = Inventory_Controller.g_ICinstance.g_iclick_Item_Count; // 클릭했던 슬롯의 아이템 갯수 값을 임시 변수에 할당
                Inventory_Controller.g_ICinstance.g_iclick_Item_Count = m_Sslot.item_Number; //클릭했던 슬롯의 아이템 갯수 변수에 현재 클릭한 슬롯 아이템 갯수 할당
                m_Sslot.item_Number = temp_count; // 현재 클릭한 슬롯 아이템 갯수에 임시 변수 값 할당

                temp = null; // 임시 변수
                temp_count = 0; // 초기화
            }
        }    
    }

    public void Show_Image(Item Clicked_Obj) // 인벤토리에 아이템을 눌렀을때 그 이미지가 마우스를 따라다니게 해줌
    {
        if (Clicked_Obj ==null) // 이미지를 클릭하지 않았다면
        {
            g_ishow_Item_Image.gameObject.SetActive(false); // 마우스를 따라다니는 이미지 꺼줌

        }
        else if (Clicked_Obj!=null) // 이미지를 클릭했다면
        {
            g_ishow_Item_Image.gameObject.SetActive(true); // 마우스를 따라다니는 이미지 활성화
        }
    }

    public void Mouse_Follow(Image show_Image) // 마우스 따라다니는 오브젝트
    {
        show_Image.sprite = Inventory_Controller.g_ICinstance.g_Iclick_Item.item_Image; // 보여줄 이미지에 선택한 오브젝트 이미지 할당

        Vector3 mouseScreenPosition = Input.mousePosition;
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mouseScreenPosition, null, out localPoint);
        show_Image.transform.localPosition = new Vector2(localPoint.x - 30, localPoint.y + 30);
    }

    public void Off_Inven()
    {
        if (m_ISr_S != null)
        {
            m_Sslot.item_Number = m_inum;
            m_Sslot.g_Ihave_item = m_ISr_S;
            m_inum = 0;
            m_ISr_S = null;
            Inventory_Controller.g_ICinstance.g_Iclick_Item = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
      //  print(m_Sslot.g_Ihave_item.name);
        //
        if (Inventory_Controller.g_ICinstance.g_Iclick_Item != null) // 선택된 아이템이 있다면
        {
            Show_Image(Inventory_Controller.g_ICinstance.g_Iclick_Item); // 함수에 클릭한 아이템 할당
            Mouse_Follow(g_ishow_Item_Image); // 함수에 마우스를 따라다닐 이미지 변수 할당
        }
        else // 선택된 아이템이 없다면
        {
            g_ishow_Item_Image.sprite = null; // 마우스를 따라다닐 이미지 변수 초기화
            g_ishow_Item_Image.gameObject.SetActive(false); // 마우스를 따라다닐 이미지 변수 끔
            m_inum = 0;
            m_ISr_S = null;
        }
    }
}
