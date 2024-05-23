using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class Ingredient : Interactable
{
    [SerializedDictionary("IngredientStage", "GameObject")]
    public SerializedDictionary<IngredientStage, GameObject> IngredientStageToVisualDict;

    public List<IngredientStage> IngredientStages;
    public IngredientStage CurrentIngredientStage;

    public int CurrentIngredientStageIndex { get; set; }


    private GameObject _owner;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        InteractableType = InteractionManager.Instance.IngredientInteractableType;

        CurrentIngredientStageIndex = IngredientStages.FindIndex(elem => elem.Equals(CurrentIngredientStage));

        foreach (KeyValuePair<IngredientStage, GameObject> pair in IngredientStageToVisualDict)
        {
            GameObject go = pair.Value;
            go.SetActive(false);
        }

        GameObject activeVisual = null;
        IngredientStageToVisualDict.TryGetValue(CurrentIngredientStage, out activeVisual);

        if(activeVisual != null) 
        {
            activeVisual.SetActive(true);
        }

        //gameObject.GetComponent<Renderer>().material = IngredientStageToMaterialDict[CurrentIngredientStage];
        //this.gameObject.GetComponent<Renderer>().material.color = Color.red;

        //var materialsCopy = gameObject.GetComponent<Renderer>().materials;
        //materialsCopy[0] = IngredientStageToVisualDict[CurrentIngredientStage];
        //gameObject.GetComponent<Renderer>().materials = materialsCopy;
    }

    // Update is called once per frame
    void Update()
    {
        if(_owner != null) 
        {
            //Follow owner
            this.transform.position = _owner.transform.position + new Vector3(0, 2, 0);
        }
    }

    public void UseStation(Station station) 
    {
        Assert.IsTrue(station.StartIngredientStage == CurrentIngredientStage);
        Assert.IsTrue(station.EndIngredientStage == IngredientStages[CurrentIngredientStageIndex + 1]);

        GameObject previousActiveVisual = null;
        IngredientStageToVisualDict.TryGetValue(CurrentIngredientStage, out previousActiveVisual);

        if (previousActiveVisual != null)
        {
            previousActiveVisual.SetActive(false);
        }

        CurrentIngredientStage = IngredientStages[++CurrentIngredientStageIndex];

        GameObject activeVisual = null;
        IngredientStageToVisualDict.TryGetValue(CurrentIngredientStage, out activeVisual);

        if (activeVisual != null)
        {
            activeVisual.SetActive(true);
        }
    }

    public void OnGetPickedUp(GameObject owner)
    {
        _owner = owner;
    }
}
