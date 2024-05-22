using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchItem : SOItemBase
{
    public override void ExecuteItemEffect(UnitEntity allyUnit)
    {
        if(GameManager.Instance.g_GameState == GameManager.GameState.BATTLE)
        {
            BattleManager btm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            
        }
    }
}
