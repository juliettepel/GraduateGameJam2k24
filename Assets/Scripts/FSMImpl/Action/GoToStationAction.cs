using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Go To Station")]
public class GoToStationAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        NavMeshAgent navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        Station station = npc.ChooseStationToGoTo();
        navMeshAgent.SetDestination(station.transform.position);
    }
}