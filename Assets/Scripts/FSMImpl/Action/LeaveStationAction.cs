using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Leave Station Action")]
public class LeaveStationAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        NPC npc = stateMachine.GetComponent<NPC>();
        npc.CurrentStation = null;
    }
}
