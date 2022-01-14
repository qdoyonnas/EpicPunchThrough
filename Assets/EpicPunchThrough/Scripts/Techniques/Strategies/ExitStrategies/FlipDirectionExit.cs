using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDirectionExit : ExitTechStrategy
{
	public FlipDirectionExit( bool inverseStates, string[] states ) : base(inverseStates, states) { }

	public override void Exit(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		tech.owner.isFacingRight = !tech.owner.isFacingRight;
	}
}
