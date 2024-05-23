using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Interactable DummyCurrentTarget;
    public Interactable CurrentTarget { get; set; }

    public Vector3 CurrentDestination { get; set; }

    private NavMeshAgent _navMeshAgent;
    private List<Interactable> _interactables;

    private int _currentInteractableIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTarget = DummyCurrentTarget;
        CurrentDestination = CurrentTarget.transform.position;

        Debug.Log("START CALLED");
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _interactables = InteractionManager.Instance.m_Interactables;
    }

    public Interactable GetNext()
    {
        var interactable = _interactables[_currentInteractableIndex];
        _currentInteractableIndex = (_currentInteractableIndex + 1) % _interactables.Count;
        return interactable;
    }

    public bool HasReached(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
