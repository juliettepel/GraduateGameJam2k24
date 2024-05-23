using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class NPC : MonoBehaviour
{
    public Vector3 CurrentDestination { get; set; }

    private NavMeshAgent _navMeshAgent;
    private Inventory _inventory;

    public Station CurrentStation;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        _inventory = gameObject.GetComponent<Inventory>();
    }


    public bool HasReached(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Interactable ChooseObjective() 
    {
        Ingredient currentIngredient = _inventory.CurrentIngredient;
        if (currentIngredient == null)
        {
            return ChooseIngredientToPickup();
        }
        else if (currentIngredient.IsReadyToServe)
        {
            return ChooseServingStationToGoTo();
        }
        else
        {
            return ChooseStationToGoTo();
        }
    }

    //For now just use the nearest, if we have time we could choose it based on which recipe goes next.
    public Ingredient ChooseIngredientToPickup()
    {
        Ingredient closestObj = null;
        float closestObjDist = Mathf.Infinity; //Arbitrarily large number to start off

        Vector3 currentPos = gameObject.transform.position;

        foreach (Interactable interactable in InteractionManager.Instance.m_Interactables) 
        {
            if (interactable.InteractableType.Equals(InteractionManager.Instance.IngredientInteractableType)) 
            {
                Vector3 objectPos = interactable.gameObject.transform.position;

                float objectDist = (objectPos - currentPos).magnitude;
                if(objectDist < closestObjDist) 
                {
                    closestObjDist = objectDist;
                    closestObj = (Ingredient)interactable;
                }
            }
        }

        if (closestObj != null) 
        {
            Debug.LogFormat("INGREDIENT {0} IS CLOSEST", closestObj.gameObject.name);
            return closestObj.GetComponent<Ingredient>();
        }

        Debug.LogWarning("No ingredient found");
        return null;
    }


    //Get the closest station that corresponds to the IngredientStage of the ingredient we're holding
    public Station ChooseStationToGoTo()
    {
        Station closestObj = null;
        float closestObjDist = Mathf.Infinity; //Arbitrarily large number to start off

        Vector3 currentPos = gameObject.transform.position;

        foreach (Interactable interactable in InteractionManager.Instance.m_Interactables)
        {
            if (interactable.InteractableType.Equals(InteractionManager.Instance.StationInteractableType))
            {
                Station station = (Station)interactable;
                Ingredient ingredientInInventory = _inventory.CurrentIngredient;

                if (station.StartIngredientStage == ingredientInInventory.CurrentIngredientStage && station.EndIngredientStage == ingredientInInventory.IngredientStages[ingredientInInventory.CurrentIngredientStageIndex + 1]) 
                {
                    Vector3 objectPos = station.gameObject.transform.position;

                    float objectDist = (objectPos - currentPos).magnitude;
                    if (objectDist < closestObjDist)
                    {
                        closestObjDist = objectDist;
                        closestObj = (Station)interactable;
                    }
                }

            }
        }

        if (closestObj != null)
        {
            Debug.LogFormat("STATION {0} IS CLOSEST", closestObj.gameObject.name);
            return closestObj.GetComponent<Station>();
        }

        Debug.LogWarning("No station found");
        return null;
    }

    //Just find nearest. Might even be only 1 serving station?
    public ServingStation ChooseServingStationToGoTo()
    {
        ServingStation closestObj = null;
        float closestObjDist = Mathf.Infinity; //Arbitrarily large number to start off

        Vector3 currentPos = gameObject.transform.position;

        foreach (Interactable interactable in InteractionManager.Instance.m_Interactables)
        {
            if (interactable.InteractableType.Equals(InteractionManager.Instance.ServingStationInteractableType))
            {
                Vector3 objectPos = interactable.gameObject.transform.position;

                float objectDist = (objectPos - currentPos).magnitude;
                if (objectDist < closestObjDist)
                {
                    closestObjDist = objectDist;
                    closestObj = (ServingStation)interactable;
                }
            }
        }

        if (closestObj != null)
        {
            Debug.LogFormat("SERVING STATION {0} IS CLOSEST", closestObj.gameObject.name);
            return closestObj.GetComponent<ServingStation>();
        }

        Debug.LogWarning("No ingredient found");
        return null;
    }
}
