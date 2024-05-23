using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Has Reached Interactable")]
public class HasReachedInteractable : Decision
{
    public InteractableType InteractableType;
    public override bool Decide(BaseStateMachine stateMachine)
    {
        InteractionComponent interactable = stateMachine.GetComponent<InteractionComponent>();

        if (interactable.bestTarget != null)
        {
            if(interactable.bestTarget.InteractableType == InteractableType) 
            {
                Debug.LogFormat("REACHED STATION OF TYPE {0}", InteractableType.ToString());

                return true;
            }
        }

        return false;
    }
}