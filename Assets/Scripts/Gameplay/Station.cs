using UnityEngine;
using UnityEngine.UI;

public class Station : Interactable
{
    public float TimeToUse = 1.0f;
    public float TimeToRepair = 1.0f;

    float currentFeedbackTime = 0;

    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;
    public float currentValue;
    public NPC CurrentNPC;

    public Slider slider;
    public GameObject IntactState;
    public GameObject SabotagedState;

    public bool InUse { get; set; } = false;

    public UsedStationEvent usedStationEvent;
    public RepairedStationEvent repairedStationEvent;

    public override void Start()
    {
        usedStationEvent.AddListener(OnStationUseDone);
        repairedStationEvent.AddListener(OnStationRepaired);

        slider.gameObject.SetActive(false);

        base.Start();
        InteractableType = InteractionManager.Instance.StationInteractableType;
        ToggleSabotagedVisuals();
    }

    // Update is called once per frame
    void Update()
    {
         if(InUse)
         {
            UpdateFeedback();
         }
    }

    public override void OnReached(NPC npc)
    {
        CurrentNPC = npc;

        if (IsSabotaged)
        {
            StartCoroutine(npc.RepairStation(this, TimeToRepair, repairedStationEvent));
        }
        else 
        {
            InUse = true;
            slider.gameObject.SetActive(true);
            StartCoroutine(npc.UseStation(this, TimeToUse, usedStationEvent));
        }
    }

    public override bool IsValidObjective()
    {
        return !IsCurrentlyAnObjective && !InUse;
    }

    private void OnStationUseDone(NPC npc)
    {
        InUse = false;
        npc.CurrentObjective = null;
        IsCurrentlyAnObjective = false;
        HideFeedback();
    }

    private void OnStationRepaired(NPC npc)
    {
        IsCurrentlyAnObjective = false;
        IsSabotaged = false;

        slider.gameObject.SetActive(true);

        ToggleSabotagedVisuals();
    }

    private void UpdateFeedback()
    {
        if (currentFeedbackTime < TimeToUse)
        {
            currentValue = Mathf.Lerp(0.0f, 1.0f, currentFeedbackTime / TimeToUse);
            slider.value = currentValue;
            currentFeedbackTime += Time.deltaTime;
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();
        ToggleSabotagedVisuals();
    }

    private void HideFeedback()
    {
        currentFeedbackTime = 0;
        slider.value = 0;
        slider.gameObject.SetActive(false);
    }

    public void ToggleSabotagedVisuals()
    {
        SabotagedState.SetActive(IsSabotaged);
        IntactState.SetActive(!IsSabotaged);
    }
}
