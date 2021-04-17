using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechEvent : EventTechStrategy
{
	bool blend = false;
	bool skipExit = false;

	public EndTechEvent( bool blend, bool skipExit )
	{
		this.blend = blend;
		this.skipExit = skipExit;
	}

	public override void OnEvent( Technique tech, AnimationEvent e)
	{
		tech.owner.TransitionTechnique(null, blend, skipExit);
	}
}

