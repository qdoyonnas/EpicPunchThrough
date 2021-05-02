using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToAimActivate : ActivateTechStrategy
{
	public override void Activate(Technique tech)
	{
		if( tech.owner.aimSegment == Agent.AimSegment.Back ) {
			tech.owner.isFacingRight = !tech.owner.isFacingRight;
		}
	}
}
