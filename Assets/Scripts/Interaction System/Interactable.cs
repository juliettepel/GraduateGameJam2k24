using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableType InteractableType { get; set; }
    
    [SerializeField] private float m_InteractionRadius = 1.0f;
    protected Color _defaultColor;
    protected Color _targetColor;

    public Transform InteractPosition;
    public GameObject InteractPrompt;

    public bool IsCurrentlyAnObjective { get; set; } = false;
    public bool IsSabotaged = false;


    public float GetInteractionRadius()
    {
        return m_InteractionRadius;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        InteractionManager.Instance.AddInteractable(this);

        if(InteractPrompt != null)
        {
            InteractPrompt.gameObject.SetActive(false);
        }

        MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
        if(cubeRenderer)
        {
            _defaultColor = cubeRenderer.material.color;
            _targetColor = new Color(_defaultColor.r / 2, _defaultColor.g / 2, _defaultColor.b / 2, 0.5f);
        }
    }

    public virtual void OnDestroy()
    {
        if(this == null)
        {
            return;
        }
        InteractionManager.Instance.RemoveInteractable(this);
    }

    public void Reset()
    {
        MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
        if (cubeRenderer)
        {
            cubeRenderer.material.color = _defaultColor;
        }
        if (InteractPrompt != null)
        {
            InteractPrompt.gameObject.SetActive(false);
        }
    }

    public void OnInteractionAvailable()
    {
        //Display a feedback here
        Debug.Log("[Interactable] - OnInteraction");
        if (!IsSabotaged)
        {
            if (InteractPrompt != null)
            {
                InteractPrompt.gameObject.SetActive(true);
            }
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                //Debug.Log(Mathf.PingPong(Time.time, 1.0f));
                Color c = Color.Lerp(_defaultColor, _targetColor, Mathf.PingPong(Time.time, 1.0f));
                meshRenderer.material.color = c;
            }
        }
    }

    public virtual void OnInteract() 
    {
        IsSabotaged = true;
        SabotageController.Instance.AddSabotaged(this);
    }


    public virtual void OnReached(NPC npc) 
    {
        npc.CurrentObjective = null;
    }

    public virtual bool IsValidObjective() { return true; }
}
