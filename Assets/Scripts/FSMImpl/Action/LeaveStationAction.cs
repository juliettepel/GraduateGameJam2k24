using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Leave Station")]
public class LeaveStationAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        NPC npc = stateMachine.GetComponent<NPC>();
        npc.CurrentStation = null;
    }
}