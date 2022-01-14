using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToAimActivate : ActivateTechStrategy
{
	public FlipToAimActivate(bool inverseStates, string[] states) : base(inverseStates, states) { }

	public override void Activate(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		if( tech.owner.aimSegment == Agent.AimSegment.Back ) {
			tech.owner.isFacingRight = !tech.owner.isFacingRight;
		}
	}
}
