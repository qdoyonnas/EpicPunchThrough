using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchExit : ExitTechStrategy
{
	Direction direction;
	float mult;

	public LaunchExit(Direction direction, float mult)
	{
		this.direction = direction;
		this.mult = mult;
	}

	public override void Exit(Technique tech)
	{
        double charge = (tech.GetBlackboardData("charge") as double?) ?? 1.0;
		Vector3 launchDirection = Utilities.GetDirectionVector(tech.owner, direction);
        Vector3 launchVector = (launchDirection * (float)(mult * charge));
        tech.owner.physicsBody.AddVelocity(launchVector);
        tech.owner.onLayer = 1;
	}
}
