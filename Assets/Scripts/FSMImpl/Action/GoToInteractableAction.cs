using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Go To Interactable")]
public class GoToInteractableAction : FSMAction
{
    public InteractableType InteractableType;
    public override void Execute(BaseStateMachine stateMachine)
    {
        NavMeshAgent navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        if (npc.HasReached(navMeshAgent))
        {
            navMeshAgent.SetDestination(npc.GetNext().transform.position);
        }
    }
}