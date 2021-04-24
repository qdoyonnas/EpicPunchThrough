using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : CameraControl
{
	public Transform target;
    public bool offsetByTargetVelocity;

    public float acceleration;
    public float dampening;
    public float friction;
    public float stopSpeed;
    public Vector3 deadZone;

    protected Vector2 velocity = new Vector2();

    protected override void DoFixedUpdate( GameManager.UpdateData data )
    {
        if( target == null ) { return; }

        Vector2 targetPosition = target.position;
        if( offsetByTargetVelocity ) {
            PhysicsBody targetBody = target.GetComponent<PhysicsBody>();
            if( targetBody != null ) {
                targetPosition += (Vector2)targetBody.velocity;
                if( Vector2.Distance(targetPosition, target.position) > (cameraBase.zoom * 0.8f) ) {
                    Vector2 direction = (targetPosition - (Vector2)target.position).normalized;
                    targetPosition = (Vector2)target.position + (direction * (cameraBase.zoom * 0.8f));
                }
            }
        }

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

    public float DistanceToTarget()
    {
        return Vector2.Distance(cameraBase.transform.position, target.position);
    }
}
