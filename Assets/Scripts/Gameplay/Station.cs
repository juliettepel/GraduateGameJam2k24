using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public float TimeToUse = 1.0f;

    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;

    public bool InUse { get; set; } = false;


    public override void Start()
    {
        base.Start();
        InteractableType = InteractionManager.Instance.StationInteractableType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnReached(NPC npc)
    {
        InUse = true;
        StartCoroutine(npc.UseStation(this, TimeToUse));
        //InUse = false;
        npc.CurrentObjective = null;
        IsCurrentlyAnObjective = false;
    }

    public override bool IsValidObjective()
    {
        return !IsCurrentlyAnObjective && !InUse;
    }
}
