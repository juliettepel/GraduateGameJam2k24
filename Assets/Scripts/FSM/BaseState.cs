using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : ScriptableObject
{
    public virtual void Execute(BaseStateMachine machine) { }
    public virtual void DoOnEnterState(BaseStateMachine machine) { }
    public virtual void DoOnExitState(BaseStateMachine machine) { }
}
