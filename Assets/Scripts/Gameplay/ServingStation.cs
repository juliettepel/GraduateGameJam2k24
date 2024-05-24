using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Count maybe put some order/sabotage/points logic here ?
public class ServingStation : Interactable
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        InteractableType = InteractionManager.Instance.ServingStationInteractableType;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnReached(NPC npc)
    {
        npc.Serve();
        npc.CurrentObjective = null;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        System.Random random = new System.Random();
        int randomIndex = random.Next(0, GameController.Instance.servingStationLocations.Length);
        gameObject.transform.position = GameController.Instance.servingStationLocations[randomIndex].position;
    }
}
