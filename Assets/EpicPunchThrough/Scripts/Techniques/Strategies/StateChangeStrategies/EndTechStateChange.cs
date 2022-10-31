using System;
using System.Collections.Generic;
using UnityEngine;

public class EndTechStateChange: StateChangeStrategy
{
    bool abortTechnique = false;

    public EndTechStateChange(bool inverseStates, string[] states, bool abortTechnique) : base(inverseStates, states)
    {
        this.abortTechnique = abortTechnique;
    }

    public override void OnStateChange( Technique tech, Agent.State previousState, Agent.State newState )
    {
        if( !ValidateState(tech) ) { return; }

        if( abortTechnique ) { tech.state = Technique.State.ABORT; }
        tech.owner.TransitionTechnique(null, false);
    }
}