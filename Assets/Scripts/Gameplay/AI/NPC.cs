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
    private List<Interactable> _interactables = new List<Interactable>();
    private Inventory _inventory;

    private int _currentInteractableIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _interactables = InteractionManager.Instance.m_Interactables;

        if (_currentInteractableIndex == 0)
        {
            _navMeshAgent.SetDestination(_interactables[0].transform.position);
        }

        _inventory = gameObject.GetComponent<Inventory>();
    }

    public Interactable GetNext()
    {
        var interactable = _interactables[_currentInteractableIndex];
        _currentInteractableIndex = (_currentInteractableIndex + 1) % _interactables.Count;
        return interactable;
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
}
