using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Decisions/Picked Up Ingredient")]
public class HasPickedUpIngredientDecision : Decision
{
    //Needed ?
    public override bool Decide(BaseStateMachine stateMachine)
    {
        Inventory inventory = stateMachine.GetComponent<Inventory>();

        if (inventory.CurrentIngredient != null)
        {
            Debug.Log("HasPickedUpIngredientDecision - Return true");
            return true;
        }

        return false;
    }
}