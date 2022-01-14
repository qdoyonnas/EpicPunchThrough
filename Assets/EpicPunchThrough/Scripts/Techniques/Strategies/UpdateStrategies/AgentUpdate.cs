using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUpdate : UpdateTechStrategy
{
	float frictionMultiplier;
    float gravityMultiplier;
	bool slideParticles;

	public AgentUpdate( bool inverseStates, string[] states, float friction, float gravity, bool slide )
		: base(inverseStates, states)
	{
		frictionMultiplier = friction;
		gravityMultiplier = gravity;
		slideParticles = slide;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( !ValidateState(tech) ) { return; }
		
		if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = slideParticles; }
        tech.owner.HandlePhysics( data, tech.owner.physicsBody.frictionCoefficients * frictionMultiplier, 
			EnvironmentManager.Instance.GetEnvironment().gravity * gravityMultiplier );
        tech.owner.HandleAnimation();
	}
}
