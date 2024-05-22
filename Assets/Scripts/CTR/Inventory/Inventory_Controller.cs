using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class Inventory_Controller : MonoBehaviour
{
    // g_fCharacterSpeed -> g�� �۷ι�(public) m�� ���(private) ���� f(float)/i(int)/s(string)
    [Header("�κ��丮 �¿����� ���� �θ� ���� �޾ƿ�")]
    public GameObject g_gin_V;
    [Header("���� �κ��丮")]
    public GameObject g_ginventory;
    public static Inventory_Controller g_ICinstance;
    public Slot[] g_Sslot; // �κ��丮 �ȿ� �ִ� �� ���Ե�
    public Slot g_Sclick_Slot; // ���� ����
    public ItemEntity g_Iget_Item; // ȹ���� ������

    public Slot g_Sselect_Item; // ���� �κ��丮���� ������ ������Ʈ ����
    //public Miri_Slot select_Item_Miri; // �̸����� �κ��丮���� ������ ������Ʈ ����
    public GameObject g_gselect_Item_Ob;

    public ItemEntity g_Iclick_Item; // Ŭ���� ������s
    public int g_iclick_Item_Count; // Ŭ���� ������ ����

    public bool invent_On_Off_Check; // �κ��丮�� �����ִ��� �����ִ��� Ȯ�����ִ� ����
    public bool lock_UI; // Ư���� UI�� ���������� �ٸ��� �ǵ��� ���ϰ� ��
    public GameObject discard_value_View; // ���� ���� �Է� â ����

    [Header("�κ��丮 ������ ǥ�� UI")]
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
        for (int i = 0; i < g_ginventory.transform.childCount; i++)  // ����Ƽ â���� ������ �־��ִ°� �ƴϰ� ��ũ��Ʈ���� �־��ִ°�
        {
            g_Sslot[i] = g_ginventory.transform.GetChild(i).GetComponent<Slot>(); // ����Ƽ�󿡼� �κ��丮��� ������Ʈ �ȿ� ���Ե��� �ֱ⶧���� �� ���Ե��� �����ͼ� �迭�� �־���
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

/*    public void Throw_Item() // �κ��丮���� Ŭ���� ������ ����
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
    public void Check_Slot(int num = 1) // ȹ���� �������� �κ��丮�� �־��ִ� �Լ�
    {
        ItemEntity item = null; // ȹ���� �������� �κ��丮�� �ִ��� �������� �Ǵ����ִ� ����
        foreach (Slot slot_B in g_Sslot)
        {
            if (slot_B != null && slot_B.g_Ihave_item != null)  // �κ��丮�� ������� �ʴٸ�
            {
                if (slot_B.g_Ihave_item.m_sItemName == g_Iget_Item.m_sItemName) // ���� ������ �ִ� �����۰� �κ��丮�� �ִ� �������� ���ٸ�
                {
                    item = g_Iget_Item; // ������ ���� ������ �ִ� �������� �־���
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
            if (item == null) //�κ��丮�� ���� ������ �ִ� �������� ���ٸ�
            {
                if (g_Sslot[i].g_Ihave_item == null) // �κ��丮�� ���������
                {
                    g_Sslot[i].Input_Item(g_Iget_Item, num); // ����ִ� ���Կ� ���� �������� �־���
                    break;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (g_Sslot[i].g_Ihave_item != null) // �κ��丮�� ������� �ʴٸ�
                {
                    if (g_Sslot[i].g_Ihave_item.m_sItemName == item.m_sItemName) // �κ��丮�� ���� ������ �ִ� �������� �ִٸ�
                    {
                        g_Sslot[i].Input_Item(g_Iget_Item, num); // ���Կ� �������� �־���
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
    public void View_Inventory() // �κ��丮 �� ���� �Լ�
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameManager.Instance.g_GameState == GameManager.GameState.BATTLE)
                return;
            if (g_gin_V.transform.localScale == new Vector3(1, 1, 1)) // �κ��丮�� ��������
            {
                Hide_Inv();
            }
            else if (g_gin_V.transform.localScale == new Vector3(0, 0, 1))
            {
                Change_UI.instance.ALL_OFF_UI();
                Show_Inv();
            }
        }
        /*
            if (g_gin_V.transform.localScale == new Vector3(1, 1, 1)) // �κ��丮�� ��������
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
            {
                Hide_Inv();
            }
        }
        else if (g_gin_V.transform.localScale == new Vector3(0, 0, 1))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Show_Inv();
            }
        }
        */
    }

    public void Set_GetItem(GameObject itemEntity)
    {
        GameObject entity = Instantiate(itemEntity,GameObject.Find("Inventory_GOs").transform);
        Destroy(entity.transform.GetComponent<SpriteRenderer>());
        Destroy(entity.transform.GetComponent<Collider2D>());
        g_Iget_Item = entity.GetComponent<ItemEntity>();
    }
    public void Show_Inv()
    {
        Img_View.gameObject.SetActive(false);
        name_View.text = " ";
        Des_View.text = " ";
        invent_On_Off_Check = true;
        g_gin_V.transform.localScale = new Vector3(1, 1, 1);
    }
    public void Hide_Inv()
    {
        for (int i = 0; i < g_ginventory.transform.childCount; i++)  // ����Ƽ â���� ������ �־��ִ°� �ƴϰ� ��ũ��Ʈ���� �־��ִ°�
        {
            g_Sslot[i].GetComponent<Slot_Button>().Off_Inven();
        }
        g_gin_V.transform.localScale = new Vector3(0, 0, 1); // ����
        invent_On_Off_Check = false;

        if (GameManager.Instance.g_GameState == GameManager.GameState.BATTLE)
            GameObject.Find("BattleManager").transform.GetComponent<BattleManager>().state = BattleManager.BattleState.ACTION;
    }
}
