using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchExit : ExitTechStrategy
{
	Direction direction;
	float mult;
	float minForce;
	float maxForce;

	public LaunchExit(Direction direction, float mult, float minForce, float maxForce)
	{
		this.direction = direction;
		this.mult = mult;
		this.minForce = minForce;
		this.maxForce = maxForce;
	}

	public override void Exit(Technique tech)
	{
		Vector3 launchDirection = Utilities.GetDirectionVector(tech.owner, direction);
		float force = Utilities.VFToForce(tech.owner.chargingVF * mult, minForce, maxForce);

        Vector3 launchVector = (launchDirection * (float)force);
        tech.owner.physicsBody.AddVelocity(launchVector);
        tech.owner.onLayer = 1;
	}
}
