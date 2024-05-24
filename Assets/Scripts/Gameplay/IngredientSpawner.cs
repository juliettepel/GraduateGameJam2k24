using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : Interactable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject ingredientPrefab;

    private Ingredient _currentIngredient;

    public GameObject IntactState;
    public GameObject SabotagedState;

    public float SabotatedTimer = 4;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        InteractableType = InteractionManager.Instance.IngredientSpawnerInteractableType;
        ToggleSabotagedVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentIngredient == null && !IsSabotaged)
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

    public override void OnInteract()
    {
        base.OnInteract();
        Destroy(_currentIngredient.gameObject);
        ToggleSabotagedVisuals();
        StartCoroutine(CountdownSabotage());
    }

    public IEnumerator CountdownSabotage()
    {
        yield return new WaitForSeconds(SabotatedTimer);

        OnSabotagedCountdownFinished();
    }

    private void OnSabotagedCountdownFinished()
    {
        IsSabotaged = false;
        ToggleSabotagedVisuals();
    }

    public void ToggleSabotagedVisuals()
    {
        SabotagedState.SetActive(IsSabotaged);
        IntactState.SetActive(!IsSabotaged);
    }
}
