using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/UseStation")]
public class UseStationAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        Inventory inventory = stateMachine.GetComponent<Inventory>();
        InteractionComponent interactionComponent = stateMachine.GetComponent<InteractionComponent>();

        InteractableType interactableType = InteractionManager.Instance.StationInteractableType;

        Interactable interactable = interactionComponent.bestTarget;
        if (interactable != null)
        {
            if (interactable.InteractableType.Equals(interactableType))
            {
                Debug.Log("USE STATION");

                Station station = (Station)interactable;
                inventory.CurrentIngredient.UseStation(station);
            }
        }
    }
}

