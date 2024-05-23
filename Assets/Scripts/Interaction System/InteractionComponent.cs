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
            if(interactable != null)
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
                    }
                    else
                    {
                        interactable.Reset();
                        _canInteract = false;
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
            Debug.Log("[Interaction Component] - Interact");
        }
    }
}
