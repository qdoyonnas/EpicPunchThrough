using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ContinueValidate : ActionValidateTechStrategy
{
    public ActionState[] actionStates;

    public ContinueValidate(params ActionState[] actionStates)
    {
        this.actionStates = actionStates;
    }

    public override bool Validate( Technique tech, Agent.Action action, float value )
    {
        foreach( ActionState actionState in actionStates ) {
            if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                tech.Exit();
                return true;
            }
        }

        return false;
    }
}
