using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonCTR : MonoBehaviour
{
    public BattleManager g_BattleManager;
    public GameManager.Action g_eAction;
    public int g_iIndex;

    // 델리게이트 선언
    delegate void OnButton(GameManager.Action action, int index);

    private void Start()
    {
        //BattleManager 할당
        g_BattleManager = GameObject.Find("BattleManager").transform.GetComponent<BattleManager>();
        // 델리게이트를 생성하고 할당
        OnButton buttonDelegate = new OnButton(g_BattleManager.OnButton);
        // Button 컴포넌트의 onClick 이벤트에 델리게이트 등록
        gameObject.transform.GetComponent<Button>().onClick.AddListener(() => buttonDelegate(g_eAction, g_iIndex));

        /*
        if (gameObject.transform.GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry_MouseOn = new EventTrigger.Entry();
        entry_MouseOn.eventID = EventTriggerType.PointerEnter;
        entry_MouseOn.callback.AddListener((data) => OnMouseEnter((PointerEventData)data));

        EventTrigger.Entry entry_MouseExit = new EventTrigger.Entry();
        entry_MouseOn.eventID = EventTriggerType.PointerExit;
        entry_MouseOn.callback.AddListener((data) => OnMouseExit((PointerEventData)data));
        gameObject.transform.GetComponent<EventTrigger>().triggers.Add(entry_MouseOn);
        gameObject.transform.GetComponent<EventTrigger>().triggers.Add(entry_MouseExit); 
        */
    }
    public void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
        g_BattleManager.g_Cursor.transform.position = gameObject.transform.position + new Vector3(155.0f, 0, 0);
    }
    public void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}