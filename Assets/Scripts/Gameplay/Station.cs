using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public float TimeToUse = 1.0f;

    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;
    public NPC currentNPC;

    public bool InUse { get; set; } = false;

    public StationEvent stationEvent;

    public override void Start()
    {
        stationEvent.AddListener(OnMyCoroutineEnded);

        base.Start();
        InteractableType = InteractionManager.Instance.StationInteractableType;
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
        currentNPC = npc;
        InUse = true;
        StartCoroutine(npc.UseStation(this, TimeToUse, stationEvent));
    }

    public override bool IsValidObjective()
    {
        return !IsCurrentlyAnObjective && !InUse;
    }

    private void OnMyCoroutineEnded(NPC npc)
    {
        InUse = false;
        npc.CurrentObjective = null;
        IsCurrentlyAnObjective = false;
        HideFeedback();
    }

    private void UpdateFeedback()
    {

    }

    private void HideFeedback()
    {

    }
}
