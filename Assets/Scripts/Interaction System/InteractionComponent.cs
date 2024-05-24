using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionComponent : MonoBehaviour
{
    public Interactable bestTarget = null;
    private bool _canInteract = false;


    void Update()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        if(InteractionManager.Instance.m_Interactables.Count == 0)
        {
            return;
        }

        foreach (Interactable interactable in InteractionManager.Instance.m_Interactables)
        {
            //Super hacky check but I dont wanna spend time redoing the "interactable" hierarchy structures.
            //Both the players and the NPCs use the interactable list
            //But when the player walks up to the fridge we want them to sabotage the fridge and not interact with the ingredient (which would do nothing)
            //So tada
            bool isIngredient = interactable.InteractableType.Equals(InteractionManager.Instance.IngredientInteractableType);

            if (interactable != null && !isIngredient)
            {
                Vector3 pos = interactable.transform.position;
                float tempDistance = Vector3.Distance(pos, position);

                if (tempDistance < distance)
                { 
                    distance = tempDistance;

                    if (distance < interactable.GetInteractionRadius())
                    {

                        interactable.OnInteractionAvailable();
                        _canInteract = true;
                        bestTarget = interactable;

                    }
                    else
                    {
                        interactable.Reset();
                        _canInteract = false;
                        bestTarget = null;
                    }

                    if (bestTarget != null)
                    {
                        Debug.Log("Interaction radius");
                    }
                }
            }
        }
    }

    public void Interact()
    {
        if(bestTarget == null)
        {
            Debug.Log("No target to interact with");
            return;
        }

        if(_canInteract)
        {
            //Interact here when a button is pressed
            bestTarget.OnInteract();
            Debug.Log("[Interaction Component] - Interact");
        }
    }
}
