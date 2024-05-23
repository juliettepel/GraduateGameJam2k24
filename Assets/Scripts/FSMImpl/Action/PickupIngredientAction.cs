using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/PickupIngredient")]
public class PickupIngredientAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        Inventory inventory = stateMachine.GetComponent<Inventory>();
        InteractionComponent interactionComponent = stateMachine.GetComponent<InteractionComponent>();
        NPC npc = stateMachine.GetComponent<NPC>();

        InteractableType interactableType = InteractionManager.Instance.IngredientInteractableType;

        Interactable interactable = interactionComponent.bestTarget;
        if (interactable != null)
        {
            if (interactable.InteractableType.Equals(interactableType))
            {
                Debug.Log("PICKED UP INGREDIENT");

                inventory.PickupIngredient((Ingredient)interactable);
            }
        }
    }
}
