using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public float TimeToUse = 1.0f;
    public float TimeToRepair = 1.0f;

    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;
    float currentValue;
    public NPC CurrentNPC;

    public bool InUse { get; set; } = false;

    public UsedStationEvent usedStationEvent;
    public RepairedStationEvent repairedStationEvent;

    public override void Start()
    {
        usedStationEvent.AddListener(OnStationUseDone);
        repairedStationEvent.AddListener(OnStationRepaired);

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
        CurrentNPC = npc;
        InUse = true;

        if (IsSabotaged)
        {
            StartCoroutine(npc.RepairStation(this, TimeToRepair, repairedStationEvent));
        }
        else 
        {
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

        ResetDefaultColor();
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
