using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Done With Station")]
public class DoneWithStationDecision : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        Inventory inventory = stateMachine.GetComponent<Inventory>();
        NPC npc = stateMachine.GetComponent<NPC>();

        if (inventory.CurrentIngredient != null)
        {
            if (npc.CurrentStation.EndIngredientStage == inventory.CurrentIngredient.CurrentIngredientStage) 
            {
                return true;
            }
        }

        return false;
    }
}
