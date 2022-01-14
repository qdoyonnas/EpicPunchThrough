using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideUpdate : UpdateTechStrategy
{
    float frictionMultiplier;
    float gravityMultiplier;

    public WallSlideUpdate( bool inverseStates, string[] states, float frictionMultiplier, float gravityMultiplier )
        : base(inverseStates, states)
    {
        this.frictionMultiplier = frictionMultiplier;
        this.gravityMultiplier = gravityMultiplier;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( !ValidateState(tech) ) { return; }

        Vector3? friction = new Vector3();
        Vector3? gravity = null;

        if( tech.owner.slideParticle != null ) {
            tech.owner.slideParticle.End();
            tech.owner.slideParticle.transform.parent = null;
        }
        tech.owner.slideParticle = ParticleManager.Instance.CreateEmitter(tech.owner.GetAnchor("FloorAnchor").position, 90f, tech.owner.transform, tech.particleController.GetParticles("slide"));

        if( tech.owner.physicsBody.velocity.y >= 0 ) {
            gravity = EnvironmentManager.Instance.GetEnvironment().gravity * gravityMultiplier;
            tech.owner.animator.SetBool("WallRunning", true);
		    if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = false; }
        } else {
            friction = tech.owner.physicsBody.frictionCoefficients * frictionMultiplier;
            tech.owner.animator.SetBool("WallRunning", false);
		    if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = true; }
        }
        

        tech.owner.HandlePhysics( data, friction, gravity );
        tech.owner.HandleAnimation();
    }
}