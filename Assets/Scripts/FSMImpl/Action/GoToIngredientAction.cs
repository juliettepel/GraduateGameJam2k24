using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Go To Ingredient")]
public class GoToIngredientAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        NavMeshAgent navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        Ingredient ingredient = npc.ChooseIngredientToPickup();
        navMeshAgent.SetDestination(ingredient.transform.position);


        //if (npc.HasReached(navMeshAgent))
        //{
        //    navMeshAgent.SetDestination(npc.GetNext().transform.position);
        //}
    }
}