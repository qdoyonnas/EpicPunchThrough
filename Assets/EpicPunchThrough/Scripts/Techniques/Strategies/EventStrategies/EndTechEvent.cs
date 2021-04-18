using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechEvent : EventTechStrategy
{
	string eventKey;
	bool blend = false;
	bool skipExit = false;

	public EndTechEvent( string key, bool blend, bool skipExit )
	{
		this.eventKey = key;
		this.blend = blend;
		this.skipExit = skipExit;
	}

	public override void OnEvent( Technique tech, AnimationEvent e)
	{
		if( !string.IsNullOrEmpty(eventKey)
			&& eventKey != e.stringParameter ) {
			return;
		}

		tech.owner.TransitionTechnique(null, blend, skipExit);
	}
}

