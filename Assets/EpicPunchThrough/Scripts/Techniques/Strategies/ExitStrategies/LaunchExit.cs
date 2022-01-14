using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchExit : ExitTechStrategy
{
	Direction direction;
	float mult;
	float minForce;
	float maxForce;

	public LaunchExit( bool inverseStates, string[] states, Direction direction, float mult, float minForce, float maxForce )
		: base(inverseStates, states)
	{
		this.direction = direction;
		this.mult = mult;
		this.minForce = minForce;
		this.maxForce = maxForce;
	}

	public override void Exit(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		Vector3 launchDirection = Utilities.GetDirectionVector(tech.owner, direction);
		float force = Utilities.VFToForce(tech.owner.chargingVF * mult, 0, maxForce) + minForce;

        Vector3 launchVector = (launchDirection * (float)force);
        tech.owner.physicsBody.AddVelocity(launchVector);
        tech.owner.onLayer = 1;
	}
}
