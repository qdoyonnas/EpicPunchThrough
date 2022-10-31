using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchEvent : EventTechStrategy
{
	string eventKey;
	Direction direction;
	float mult;
	float minForce;
	float maxForce;

	public LaunchEvent( bool inverseStates, string[] states, string key, Direction direction, float mult, float minForce, float maxForce )
		: base(inverseStates, states)
	{
		this.eventKey = key;
		this.direction = direction;
		this.mult = mult;
		this.minForce = minForce;
		this.maxForce = maxForce;
	}

	public override void OnEvent(Technique tech, AnimationEvent e)
	{
		if( !ValidateState(tech) ) { return; }

		if( !string.IsNullOrEmpty(eventKey)
			&& eventKey != e.stringParameter ) {
			return;
		}

		Vector3 launchDirection = Utilities.GetDirectionVector(tech.owner, direction);
		float force = Utilities.VFToForce(tech.owner.chargingVF * mult, 0, maxForce) + minForce;

        Vector3 launchVector = (launchDirection * (float)force);
        tech.owner.physicsBody.AddVelocity(launchVector);
        tech.owner.onLayer = 1;
	}
}
