using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactable
{
    public IngredientStage IngredientStage;
    public bool IsDone { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        IsDone = false;
        InteractableType = InteractionManager.Instance.StationInteractableType;
    }

    // Update is called once per frame
    void Update()
    {
        
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
