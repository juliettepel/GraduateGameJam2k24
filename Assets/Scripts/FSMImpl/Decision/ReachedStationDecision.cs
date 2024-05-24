using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Reached Station")]
public class ReachedStationDecision : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        NPC npc = stateMachine.GetComponent<NPC>();

        if (npc.CurrentStation != null)
        {
            return true;
        }

        return false;
    }
}
