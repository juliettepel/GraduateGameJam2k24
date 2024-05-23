using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableType InteractableType { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        InteractionManager.Instance.AddInteractable(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
