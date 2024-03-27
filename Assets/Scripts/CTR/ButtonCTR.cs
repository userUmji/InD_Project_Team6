using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        gameObject.GetComponent<Button>().onClick.AddListener(() => buttonDelegate(g_eAction, g_iIndex));
    }
}