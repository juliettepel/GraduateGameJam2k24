using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/UseStation")]
public class UseStationAction : FSMAction
{

    //? Maybe not like this
    public IngredientStage StartStage;
    public IngredientStage EndStage;
    public override void Execute(BaseStateMachine stateMachine)
    {
        //TODO
    }
}

