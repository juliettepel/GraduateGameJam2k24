using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableType InteractableType { get; set; }
    
    [SerializeField] private float m_InteractionRadius = 1.0f;
    Color defaultColor;
    Color targetColor;

    public Transform InteractPosition;

    public bool IsCurrentlyAnObjective { get; set; } = false;
    public bool IsSabotaged { set; get; } = false;


    public float GetInteractionRadius()
    {
        return m_InteractionRadius;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        InteractionManager.Instance.AddInteractable(this);

        MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
        if(cubeRenderer)
        {
            defaultColor = cubeRenderer.material.color;
            targetColor = new Color(defaultColor.r / 2, defaultColor.g / 2, defaultColor.b / 2, 0.5f);
        }
    }

    void OnDestroy()
    {
        InteractionManager.Instance.RemoveInteractable(this);
    }

    public void Reset()
    {
        MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
        if (cubeRenderer)
        {
            cubeRenderer.material.color = defaultColor;
        }
    }

    public void OnInteractionAvailable()
    {
        //Display a feedback here
        Debug.Log("[Interactable] - OnInteraction");

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            //Debug.Log(Mathf.PingPong(Time.time, 1.0f));
            Color c = Color.Lerp(defaultColor, targetColor, Mathf.PingPong(Time.time, 1.0f));
            meshRenderer.material.color = c;
        }
    }

    public virtual void OnInteract() { }

    public virtual void OnReached(NPC npc) 
    {
        npc.CurrentObjective = null;
    }

    public virtual bool IsValidObjective() { return true; }
}
