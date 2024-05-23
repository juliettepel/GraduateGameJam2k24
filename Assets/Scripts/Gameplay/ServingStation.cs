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
    }
}
