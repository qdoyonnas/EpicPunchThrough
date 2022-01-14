using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechValidate : ActionValidateTechStrategy
{
    bool abortTechnique = false;
    ActionState[] actionStates;

    public EndTechValidate(bool inverseStates, string[] states, bool abortTechnique, params ActionState[] actionStates)
        : base(inverseStates, states)
    {
        this.abortTechnique = abortTechnique;
        this.actionStates = actionStates;
    }

    public override bool Validate( Technique tech, Agent.Action action, float value )
    {
        if( !ValidateState(tech) ) { return false; }

        foreach( ActionState actionState in actionStates ) {
            if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                tech.owner.TransitionTechnique(null, true);
                return true;
            }
        }

        return false;
    }
}
