using System;
using System.Collections.Generic;
using UnityEngine;

public class EndTechStateChange: StateChangeStrategy
{
    bool skipExit = true;

    public override void OnStateChange( Technique tech, Agent.State previousState, Agent.State newState )
    {
        tech.owner.TransitionTechnique(null, false, skipExit);
    }
}