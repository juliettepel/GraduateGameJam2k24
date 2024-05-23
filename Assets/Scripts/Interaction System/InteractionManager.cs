using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    public List<Interactable> m_Interactables = new List<Interactable>();

    public InteractableType StationInteractableType;
    public InteractableType IngredientInteractableType;
    public InteractableType ServingStationInteractableType;


    public void AddInteractable(Interactable interactable)
    {
        m_Interactables.Add(interactable);
    }
}
