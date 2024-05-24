using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableType InteractableType { get; set; }
    
    [SerializeField] private float m_InteractionRadius = 1.0f;
    protected Color _defaultColor;
    protected Color _targetColor;

    public GameObject IntactState;
    public GameObject SabotagedState;

    public Transform InteractPosition;

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

        MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
        if(cubeRenderer)
        {
            _defaultColor = cubeRenderer.material.color;
            _targetColor = new Color(_defaultColor.r / 2, _defaultColor.g / 2, _defaultColor.b / 2, 0.5f);
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
            cubeRenderer.material.color = _defaultColor;
        }
    }

    public void OnInteractionAvailable()
    {
        //Display a feedback here
        Debug.Log("[Interactable] - OnInteraction");

        if(!IsSabotaged)
        {
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

    //private void ToggleSabotagedColorOn()
    //{
    //    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    //    if (meshRenderer)
    //    {
    //        meshRenderer.material.color = SabotagedColor;
    //    }
    //}

    //public void ResetDefaultColor()
    //{
    //    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    //    if (meshRenderer)
    //    {
    //        meshRenderer.material.color = _defaultColor;
    //    }
    //}

    public virtual bool IsValidObjective() { return true; }
}
