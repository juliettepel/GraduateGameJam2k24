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

        foreach (Interactable interactable in InteractionManager.Instance.m_Interactables)
        {
            if(interactable != null)
            {
                Vector3 posAnimal = interactable.transform.position;
                float tempDistance = Vector3.Distance(posAnimal, position);

                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    if (distance < interactable.GetInteractionRadius())
                    {
                        interactable.OnInteractionAvailable();
                        _canInteract = true;
                    }

                    _canInteract = false;

                    if (bestTarget != null)
                    {

                    }
                    bestTarget = interactable;
                }
            }
        }
    }

    void Interact()
    {
        if(bestTarget == null)
        {
            Debug.Log("No target to interact with");
            return;
        }

        if(_canInteract)
        {
            //Interact here when a button is pressed

        }
    }
}
