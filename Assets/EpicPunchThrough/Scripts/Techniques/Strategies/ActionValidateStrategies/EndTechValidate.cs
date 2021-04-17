using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechValidate : ActionValidateTechStrategy
{
    ActionState[] actionStates;
    bool skipExit = false;

    public EndTechValidate(bool skipExit, params ActionState[] actionStates)
    {
        this.skipExit = skipExit;
        this.actionStates = actionStates;
    }

    public override bool Validate( Technique tech, Agent.Action action, float value )
    {
        foreach( ActionState actionState in actionStates ) {
            if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                tech.owner.TransitionTechnique(null, true, skipExit);
                return true;
            }
        }

        return false;
    }
}
