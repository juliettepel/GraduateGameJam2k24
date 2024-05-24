using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public float TimeToUse = 1.0f;

    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;
    public NPC currentNPC;
    float currentValue;

    public bool InUse { get; set; } = false;

    public UsedStationEvent usedStationEvent;

    public override void Start()
    {
        usedStationEvent.AddListener(OnStationUseDone);

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
        StartCoroutine(npc.UseStation(this, TimeToUse, usedStationEvent));
    }

    public override void OnInteract()
    {
        IsSabotaged = true;
        SabotageController.Instance.AddSabotaged(this);
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

    private void UpdateFeedback()
    {
        float time = 0;
        while (time < TimeToUse)
        {
            currentValue = Mathf.Lerp(0, 1, time / TimeToUse);
            Debug.Log("LERP" + currentValue);
            time += Time.deltaTime;
        }

    }

    private void HideFeedback()
    {

    }
}
