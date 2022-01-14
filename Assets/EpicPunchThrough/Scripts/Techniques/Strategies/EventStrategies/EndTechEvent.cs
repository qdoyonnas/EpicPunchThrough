using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechEvent : EventTechStrategy
{
	string eventKey;
	bool blend = false;

	public EndTechEvent( bool inverseStates, string[] states, string key, bool blend, bool skipExit )
		: base(inverseStates, states)
	{
		this.eventKey = key;
		this.blend = blend;
	}

	public override void OnEvent( Technique tech, AnimationEvent e)
	{
		if( !ValidateState(tech) ) { return; }

		if( !string.IsNullOrEmpty(eventKey)
			&& eventKey != e.stringParameter ) {
			return;
		}

		tech.owner.TransitionTechnique(null, blend);
	}
}

