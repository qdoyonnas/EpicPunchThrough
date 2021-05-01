using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : CameraControl
{
    public float offsetByTargetVelocity;

    public float acceleration;
    public float dampening;
    public float friction;
    public float stopSpeed;
    public Vector2 deadZone;
    public Vector2 velocityClamp;

    protected Vector2 oldOffset = new Vector2();
    protected Vector2 velocity = new Vector2();

    protected override void DoFixedUpdate( GameManager.UpdateData data )
    {
        if( target == null ) { return; }

        Vector2 targetPosition = GetTargetPosition();

        Vector2 offset = targetPosition - (Vector2)cameraBase.transform.position;

        if( Mathf.Sign(velocity.x) != Mathf.Sign(offset.x) ) {
            velocity.x = velocity.x * (1 - dampening);
        }
        velocity.x += (offset.x / deadZone.x) * acceleration * data.deltaTime;
        
        if( Mathf.Sign(velocity.y) != Mathf.Sign(offset.y) ) {
            velocity.y = velocity.y * (1 - dampening);
        }
        velocity.y += (offset.y / deadZone.y) * acceleration * data.deltaTime;

        if( velocity.magnitude > 0 ) {
            velocity = velocity * (1 - friction);
            if( velocity.magnitude < stopSpeed ) {
                velocity = new Vector2();
            }
        }

        cameraBase.Move(velocity, true);
    }

    protected virtual Vector2 GetTargetPosition()
    {
        Vector2 targetPosition = target.position;
        if( offsetByTargetVelocity > 0 ) {
            PhysicsBody targetBody = target.GetComponent<PhysicsBody>();
            if( targetBody != null ) {
                Vector2 offset = (Vector2)targetBody.velocity * offsetByTargetVelocity;

                if( Mathf.Abs(offset.x) > (cameraBase.zoom * velocityClamp.x) ) {
                    offset = new Vector2(Mathf.Sign(offset.x) * (cameraBase.zoom * velocityClamp.x), offset.y);
                }
                if( Mathf.Abs(offset.y) > (cameraBase.zoom * velocityClamp.y) ) {
                    offset = new Vector2(offset.x, Mathf.Sign(offset.y) * (cameraBase.zoom * velocityClamp.y));
                }

                float ratio = Mathf.Min(offset.magnitude/cameraBase.zoom, 1);
                targetPosition = (Vector2)target.position + (offset * ratio);
            }
        }

        return targetPosition;
    }

    public float DistanceToTarget()
    {
        return Vector2.Distance(cameraBase.transform.position, target.position);
    }
}
