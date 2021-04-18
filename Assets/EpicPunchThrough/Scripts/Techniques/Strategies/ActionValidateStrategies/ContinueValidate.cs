using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ContinueValidate : ActionValidateTechStrategy
{
    bool onlyBeforeExit = false;
    public ActionState[] actionStates;

    public ContinueValidate(bool beforeExit, params ActionState[] actionStates)
    {
        this.onlyBeforeExit = beforeExit;
        this.actionStates = actionStates;
    }

    public override bool Validate( Technique tech, Agent.Action action, float value )
    {
        if( onlyBeforeExit && tech.state >= Technique.State.Exit ) { return false; }

        foreach( ActionState actionState in actionStates ) {
            if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                tech.Exit();
                return true;
            }
        }

        return false;
    }
}
