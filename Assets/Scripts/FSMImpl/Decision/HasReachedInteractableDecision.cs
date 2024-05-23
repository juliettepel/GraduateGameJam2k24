using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Decisions/Has Reached Interactable")]
public class HasReachedInteractableDecision : Decision
{
    //Needed ?
    public InteractableType InteractableType;
    public override bool Decide(BaseStateMachine stateMachine)
    {
        NavMeshAgent navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        if (npc.HasReached(navMeshAgent))
        {
            Debug.Log("HasReachedInteractableDecision - REACHED");
            return true;
        }

        return false;
    }
}