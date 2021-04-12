using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveUpdate : UpdateTechStrategy
{
    Direction direction;
    float maxSpeed;
    float acceleration;
    
    public MoveUpdate(Direction direction, float maxSpeed, float acceleration)
    {
        this.direction = direction;
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( Mathf.Abs(tech.owner.physicsBody.velocity.x) < maxSpeed )
        {
            Vector3 velDirection = Utilities.GetDirectionVector(tech.owner, direction);
            Vector3 velDelta = velDirection * (acceleration * data.deltaTime * value);

            tech.owner.physicsBody.AddVelocity(velDelta);
        }

        tech.owner.HandlePhysics( data, new Vector3(0, 0, 0) );
        tech.owner.HandleAnimation();
    }
}

[Serializable]
public class MoveUpdateOptions : UpdateTechStrategyOptions {
    public Direction direction;
    public float maxSpeed;
    public float acceleration;

    public override void InspectorDraw()
    {
        direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
        maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
        acceleration = EditorGUILayout.FloatField("Acceleration", acceleration);
    }

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new MoveUpdate(direction, maxSpeed, acceleration);
    }
}
