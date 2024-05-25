using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Count maybe put some order/sabotage/points logic here ?
public class ServingStation : Interactable
{
    public bool CanBeSabotaged { get; set; } = true;
    public float SabotageCooldown = 4;
    public float currentSliderValue;
    float currentTimerValue = 0;

    public AudioSource SabotageAudio;
    private AudioSource _sabotageAudio;
    public AudioSource FixedAudio;
    private AudioSource _fixedAudio;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        InteractableType = InteractionManager.Instance.ServingStationInteractableType;

        _sabotageAudio = Instantiate(SabotageAudio);
        _fixedAudio = Instantiate(FixedAudio);
    }


    // Update is called once per frame
    void Update()
    {
        if(IsSabotaged)
        {
            UpdateFeedback();
        }
    }

    private void UpdateFeedback()
    {
        if (currentTimerValue < SabotageCooldown)
        {
            currentSliderValue = Mathf.Lerp(0.0f, 1.0f, currentTimerValue / SabotageCooldown);
            currentTimerValue += Time.deltaTime;
        }
    }

    public override void OnReached(NPC npc)
    {
        npc.Serve();
        npc.CurrentObjective = null;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if(CanBeSabotaged)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, GameController.Instance.servingStationLocations.Length);
            gameObject.transform.position = GameController.Instance.servingStationLocations[randomIndex].position;

            OnSabotaged();
        }
    }

    private void OnSabotaged()
    {
        CanBeSabotaged = false;
        IsSabotaged = true;
        StartCoroutine(StartSabotageCooldown());
    }

    public IEnumerator StartSabotageCooldown()
    {
        CanBeSabotaged = false;
        _sabotageAudio.Play();
        yield return new WaitForSeconds(SabotageCooldown);

        CanBeSabotaged = true;
        IsSabotaged = false;
        currentTimerValue = 0;
        _fixedAudio.Play();
    }
}
