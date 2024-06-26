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

    public AudioSource SabotageAudio;
    private AudioSource _sabotageAudio;
    public AudioSource FixedAudio;
    private AudioSource _fixedAudio;

    public override void Start()
    {
        usedStationEvent.AddListener(OnStationUseDone);
        repairedStationEvent.AddListener(OnStationRepaired);

        slider.gameObject.SetActive(false);

        base.Start();
        InteractableType = InteractionManager.Instance.StationInteractableType;
        ToggleSabotagedVisuals();

        _sabotageAudio = Instantiate(SabotageAudio);
        _fixedAudio = Instantiate(FixedAudio);
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
        InUse = true;
        slider.gameObject.SetActive(true);

        ToggleSabotagedVisuals();

        _fixedAudio.Play();
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

        _sabotageAudio.Play(); 
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
