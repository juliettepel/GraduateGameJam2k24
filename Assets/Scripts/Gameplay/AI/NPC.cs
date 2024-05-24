using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Vector3 CurrentDestination { get; set; }

    private NavMeshAgent _navMeshAgent;
    private Inventory _inventory;
    public float ReachObjectiveVelocityTolerance = 0;
    public Station CurrentStation;
    public Interactable CurrentObjective { get; set; } = null;

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
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= ReachObjectiveVelocityTolerance)
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

    public void ChooseObjective() 
    {
        if (CurrentObjective != null) 
        {
            Debug.LogFormat("{0} has objective {1}", gameObject.name, CurrentObjective.gameObject.name);
            return;
        }

        if (_inventory.CurrentIngredient == null)
        {
            CurrentObjective = ChooseIngredientToPickup();
        }
        else if (_inventory.CurrentIngredient.IsReadyToServe)
        {
            CurrentObjective = ChooseServingStationToGoTo();
        }
        else
        {
            CurrentObjective =  ChooseStationToGoTo();
        }

        Debug.LogFormat("{0} chose objective {1}", gameObject.name, CurrentObjective.gameObject.name);

        CurrentObjective.IsCurrentlyAnObjective = true;
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
                Ingredient ingredient = (Ingredient)interactable;
                if(ingredient.IsValidObjective()) 
                {
                    Vector3 objectPos = interactable.gameObject.transform.position;

                    float objectDist = (objectPos - currentPos).magnitude;
                    if (objectDist < closestObjDist)
                    {
                        closestObjDist = objectDist;
                        closestObj = ingredient;
                    }
                }
            }
        }

        if (closestObj != null) 
        {
            return closestObj;
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
                    if (station.IsValidObjective())
                    {
                        Vector3 objectPos = station.gameObject.transform.position;

                        float objectDist = (objectPos - currentPos).magnitude;
                        if (objectDist < closestObjDist)
                        {
                            closestObjDist = objectDist;
                            closestObj = station;
                        }
                    }
                }
            }
        }

        if (closestObj != null)
        {
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
            return closestObj.GetComponent<ServingStation>();
        }

        Debug.LogWarning("No ingredient found");
        return null;
    }

    public void PickupIngredient(Ingredient ingredient) 
    {
        _inventory.PickupIngredient(ingredient);
    }

    void OnStationUsed(Station station)
    {
        station.InUse = false;
        _inventory.CurrentIngredient.DoProcessAtStation(station);
        CurrentStation = null;
    }

    public IEnumerator UseStation(Station station, float seconds)
    {
        CurrentStation = station;
        yield return new WaitForSeconds(seconds);

        OnStationUsed(station);
    }

    //Yas queen serve
    public void Serve()
    {
        Debug.LogFormat("{0} served {1}", gameObject.name, _inventory.CurrentIngredient.name);
        Destroy(_inventory.CurrentIngredient.gameObject);
        _inventory.CurrentIngredient = null;

        GameController.Instance.OrdersServed++;
    }

    //todo - pretty sure I didnt implement this properly
    private IEnumerator Countdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("USING STATION TIMER!");
    }
}
