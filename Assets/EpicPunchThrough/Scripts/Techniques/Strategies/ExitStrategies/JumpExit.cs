using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpExit : ExitTechStrategy
{
    float jumpMultiplier;
    float minForce;
    float maxForce;

    public JumpExit( bool inverseStates, string[] states, float jumpMultiplier, float minForce, float maxForce )
        : base(inverseStates, states)
    {
        this.jumpMultiplier = jumpMultiplier;
        this.minForce = minForce;
        this.maxForce = maxForce;
    }

    public override void Exit( Technique tech )
    {
        if( !ValidateState(tech) ) { return; }

        Transform rightFootAnchor = tech.owner.GetAnchor("FootR");
        Transform leftFootAnchor = tech.owner.GetAnchor("FootL");
        Vector3 emitterPosition = ( rightFootAnchor.position + leftFootAnchor.position ) / 2f;
        
        float force = Utilities.VFToForce(tech.owner.chargingVF * jumpMultiplier, minForce, maxForce);

        float mult =  force / 15f;
        tech.CreateEmitter("launch", emitterPosition, Vector3.SignedAngle(Vector3.up, tech.owner.aimDirection, Vector3.forward) )
            .Expand(mult)
            .Accelerate(mult);
    }
}
