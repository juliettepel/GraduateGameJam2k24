using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : Interactable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject ingredientPrefab;

    private Ingredient _currentIngredient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentIngredient == null) 
        {
            SpawnIngredient();
        }
    }

    void SpawnIngredient()
    {
        _currentIngredient = Instantiate(ingredientPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Ingredient>();
        _currentIngredient.Spawner = this;
    }

    public void OnIngredientPickedUp()
    {
        _currentIngredient = null;
    }
}
