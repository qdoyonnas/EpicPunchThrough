using System;
using System.Collections.Generic;
using UnityEngine;

public class EndTechStateChange: StateChangeStrategy
{
    public EndTechStateChange(bool inverseStates, string[] states) : base(inverseStates, states) { }

    public override void OnStateChange( Technique tech, Agent.State previousState, Agent.State newState )
    {
        if( !ValidateState(tech) ) { return; }

        tech.owner.TransitionTechnique(null, false);
    }
}