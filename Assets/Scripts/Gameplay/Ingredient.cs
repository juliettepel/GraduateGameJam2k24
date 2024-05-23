using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ingredient : Interactable
{
    public List<IngredientStage> IngredientStages;
    public IngredientStage CurrentIngredientStage;
    public int CurrentIngredientStageIndex { get; set; }

    public bool IsCooked { get; set; }

    private GameObject _owner;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        InteractableType = InteractionManager.Instance.IngredientInteractableType;

        CurrentIngredientStageIndex = IngredientStages.FindIndex(elem => elem.Equals(CurrentIngredientStage));
        IsCooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_owner != null) 
        {
            //Follow owner
            this.transform.position = _owner.transform.position;
        }
    }

    public void UseStation(Station station) 
    {
        if(station.IngredientStage == CurrentIngredientStage)
        {
            CurrentIngredientStage = IngredientStages[++CurrentIngredientStageIndex];
        }
    }

    public void OnGetPickedUp(GameObject owner)
    {
        _owner = owner;
    }
}
