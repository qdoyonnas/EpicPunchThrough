using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaunchExit : ExitTechStrategy
{
	Direction direction;
	AnimationCurve forceCurve;
	float force;

	public StaticLaunchExit( Direction direction, AnimationCurve forceCurve, float force )
	{
		this.direction = direction;
		this.forceCurve = forceCurve;
		this.force = force;
	}

	public override void Exit(Technique tech)
	{
		double charge = tech.GetBlackboardData("charge") as double? ?? 0.0;

		float forceRatio = forceCurve.Evaluate((float)charge);
		float forceTotal = force * forceRatio;
		
		Vector3 launchDirection = Utilities.GetDirectionVector(tech.owner, direction);

        Vector3 launchVector = (launchDirection * forceTotal);
        tech.owner.physicsBody.AddVelocity(launchVector);
	}
}
