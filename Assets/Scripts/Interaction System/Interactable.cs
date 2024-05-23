using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float m_InteractionRadius = 1.0f;

    public float GetInteractionRadius()
    {
        return m_InteractionRadius;
    }

    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.Instance.AddInteractable(this);
    }

    public void OnInteractionAvailable()
    {
        //Display a feedback here
        Debug.Log("[Interactable] - OnInteraction");
    }
}
