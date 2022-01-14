using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpdate : UpdateTechStrategy
{
    Direction direction;
    float maxSpeed;
    float acceleration;
    
    public MoveUpdate( bool inverseStates, string[] states, Direction direction, float maxSpeed, float acceleration )
        : base(inverseStates, states)
    {
        this.direction = direction;
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( !ValidateState(tech) ) { return; }

        if( Mathf.Abs(tech.owner.physicsBody.velocity.x) < maxSpeed )
        {
            Vector3 velDirection = Utilities.GetDirectionVector(tech.owner, direction);
            Vector3 velDelta = velDirection * (acceleration * data.deltaTime * value);

            tech.owner.physicsBody.AddVelocity(velDelta);
            if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = false; }
        }
    }
}