using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSpawner : Interactable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject ingredientPrefab;

    private Ingredient _currentIngredient;

    public GameObject IntactState;
    public GameObject SabotagedState;

    public float SabotatedTimer = 4.0f;
    public float TimeUntilNewIngredient = 4.0f;
    public float currentFeedbackTime = 0.0f;
    public float currentValue = 0.0f;

    public Slider slider;

    public bool CanSpawnIngredient = true;

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
        if(!CanSpawnIngredient)
        {
            if (currentFeedbackTime < TimeUntilNewIngredient)
            {
                currentValue = Mathf.Lerp(0.0f, 1.0f, currentFeedbackTime / TimeUntilNewIngredient);
                slider.value = currentValue;
                currentFeedbackTime += Time.deltaTime;
            }
        }
        if(_currentIngredient == null && !IsSabotaged && CanSpawnIngredient)
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
        CanSpawnIngredient = false;
    }

    public IEnumerator CountdownSabotage()
    {
        yield return new WaitForSeconds(SabotatedTimer);

        OnSabotagedCountdownFinished();
    }

    public IEnumerator CountdownIngredient()
    {
        yield return new WaitForSeconds(TimeUntilNewIngredient);

        OnNewIngredientCountdownFinished();
    }

    private void OnNewIngredientCountdownFinished()
    {
        CanSpawnIngredient = true;
        currentFeedbackTime = 0;
        slider.value = 0;
    }
    private void OnSabotagedCountdownFinished()
    {
        IsSabotaged = false;
        ToggleSabotagedVisuals();
        StartCoroutine(CountdownIngredient());
    }

    public void ToggleSabotagedVisuals()
    {
        SabotagedState.SetActive(IsSabotaged);
        IntactState.SetActive(!IsSabotaged);
    }
}
