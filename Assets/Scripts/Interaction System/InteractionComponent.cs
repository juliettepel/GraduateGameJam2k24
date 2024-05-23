using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionComponent : MonoBehaviour
{
    public Interactable bestTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
                    if (distance < 1.0f)
                    {
                        Debug.Log("Interaction radius");
                    }

                    if (bestTarget != null)
                    {

                    }
                    bestTarget = interactable;
                }
            }
        }
    }
}
