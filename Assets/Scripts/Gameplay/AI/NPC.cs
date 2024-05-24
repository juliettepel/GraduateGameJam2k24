using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour
{
    public Vector3 CurrentDestination { get; set; }

    private NavMeshAgent _navMeshAgent;
    private Inventory _inventory;
    public float ReachObjectiveVelocityTolerance = 0;
    public Station CurrentStation;
    public Interactable CurrentObjective { get; set; } = null;

    public AudioSource OrderServedAudio;
    private AudioSource _orderServedAudio;

    public AudioSource RepairNeededNoticedAudio;
    private AudioSource _repairNeededNoticedAudio;

    private bool _hasPlayedRepairNeededAudio = false;

    private bool _isRepairingStation = false;
    private float _totalTimeToRepairStation = 0.0f;
    private float _currentRepairTime = 0.0f;
    public UnityEngine.UI.Slider fixingStationSlider;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        _inventory = gameObject.GetComponent<Inventory>();

        _orderServedAudio = Instantiate(OrderServedAudio);
        _repairNeededNoticedAudio = Instantiate(RepairNeededNoticedAudio);

        _isRepairingStation = false;
        _totalTimeToRepairStation = 0.0f;
        _currentRepairTime = 0.0f;
        fixingStationSlider.gameObject.SetActive(false);
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
        if (_isRepairingStation)
        {
            RepairStationFeedback();
        }
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

    void OnStationRepaired(Station station)
    {
        StartCoroutine(UseStation(station, station.TimeToUse, station.usedStationEvent));
        _hasPlayedRepairNeededAudio = false;
        _isRepairingStation = false;
        _totalTimeToRepairStation = 0.0f;
        _currentRepairTime = 0.0f;
        fixingStationSlider.gameObject.SetActive(false);
    }

    public IEnumerator UseStation(Station station, float seconds, UsedStationEvent usedStationEvent)
    {
        CurrentStation = station;
        CurrentStation.InUse = true;
        yield return new WaitForSeconds(seconds);

        usedStationEvent.Invoke(CurrentStation.CurrentNPC);

        OnStationUsed(station);
    }

    public IEnumerator RepairStation(Station station, float seconds, RepairedStationEvent repairedStationEvent)
    {
        CurrentStation = station;
        if (!_hasPlayedRepairNeededAudio)
        {
            _repairNeededNoticedAudio.Play();
            _hasPlayedRepairNeededAudio = true;
        }
        _totalTimeToRepairStation = seconds;
        _isRepairingStation = true;
        fixingStationSlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(seconds);

        repairedStationEvent.Invoke(CurrentStation.CurrentNPC);

        OnStationRepaired(station);
    }

    //Yas queen serve
    public void Serve()
    {
        Debug.LogFormat("{0} served {1}", gameObject.name, _inventory.CurrentIngredient.name);
        Destroy(_inventory.CurrentIngredient.gameObject);
        _inventory.CurrentIngredient = null;

        GameController.Instance.OrdersServed++;

        _orderServedAudio.Play();
    }

    public void RepairStationFeedback()
    {
        if (_currentRepairTime < _totalTimeToRepairStation)
        {
            float currentSliderValue = Mathf.Lerp(0.0f, 1.0f, _currentRepairTime / _totalTimeToRepairStation);
            fixingStationSlider.value = currentSliderValue;
            _currentRepairTime += Time.deltaTime;
        }
    }
}
