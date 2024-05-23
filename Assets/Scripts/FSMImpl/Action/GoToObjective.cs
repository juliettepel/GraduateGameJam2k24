using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Go To Objective")]
public class GoToObjectiveAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        NavMeshAgent navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        Interactable objective = npc.ChooseObjective();

        navMeshAgent.SetDestination(objective.transform.position);
    }
}