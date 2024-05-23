using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public IngredientStage StartIngredientStage;
    public IngredientStage EndIngredientStage;


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
        npc.UseStation(this);
    }

    //public bool UseStation(Ingredient ingredient)
    //{
    //    if (ingredientStage != IngredientStage)
    //    {
    //        return false;
    //    }

    //    IsDone = true;

    //    return true;
    //}
}
