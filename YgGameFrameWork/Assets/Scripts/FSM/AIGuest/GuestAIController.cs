using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 客人AI控制器
/// </summary>
public class GuestAIController : AIController
{
    protected override void ConstructFSM()
    {
        DrinkingState drink = new DrinkingState();

        AddFSMState(drink);
    }
}
