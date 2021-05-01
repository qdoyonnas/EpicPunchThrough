using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExactCamera : CameraControl
{
    public bool offsetByTargetVelocity;

    public Vector2 deadZone;
    public Vector2 velocityClamp;

    protected Vector2 oldOffset = new Vector2();
    protected Vector2 velocity = new Vector2();

    protected override void DoFixedUpdate( GameManager.UpdateData data )
    {
        if( target == null ) { return; }

        Vector2 targetPosition = GetTargetPosition();

        cameraBase.Move(targetPosition, false);
    }

    protected virtual Vector2 GetTargetPosition()
    {
        Vector2 targetPosition = target.position;
        if( offsetByTargetVelocity ) {
            PhysicsBody targetBody = target.GetComponent<PhysicsBody>();
            if( targetBody != null ) {
                targetPosition += (Vector2)targetBody.velocity;

                Vector2 difference = (targetPosition - (Vector2)target.position);
                Vector2 offset = (Vector2)targetBody.velocity;
                if( Mathf.Abs(difference.x) > (cameraBase.zoom * velocityClamp.x) ) {
                    offset = new Vector2(Mathf.Sign(offset.x) * (cameraBase.zoom * velocityClamp.x), offset.y);
                }
                if( Mathf.Abs(difference.y) > (cameraBase.zoom * velocityClamp.y) ) {
                    offset = new Vector2(offset.x, Mathf.Sign(offset.y) * (cameraBase.zoom * velocityClamp.y));
                }

				oldOffset = offset;
                 targetPosition = (Vector2)target.position + (offset);
            }
        }

        return targetPosition;
    }
}
