using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SabotageController : Singleton<SabotageController>
{
    public List<Interactable> SabotagedInteractables = new List<Interactable>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSabotaged(Interactable interactable)
    {
        SabotagedInteractables.Add(interactable);
    }

    public void RemoveSabotaged(Interactable interactable)
    {
        SabotagedInteractables.Remove(interactable);
    }
}
