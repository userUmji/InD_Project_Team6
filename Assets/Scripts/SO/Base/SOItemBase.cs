using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOItemBase : ScriptableObject, IItemBehavior
{
    public abstract void ExecuteItemEffect(UnitEntity allyUnit);
} 
