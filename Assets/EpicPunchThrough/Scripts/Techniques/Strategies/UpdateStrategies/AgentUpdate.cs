using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUpdate : UpdateTechStrategy
{
	float frictionMultiplier;

	public AgentUpdate( float friction ) {
		frictionMultiplier = friction;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = true; }
        tech.owner.HandlePhysics( data, tech.owner.physicsBody.frictionCoefficients * frictionMultiplier );
	}
}
