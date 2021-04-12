using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JumpExit : ExitTechStrategy
{
    float jumpMultiplier;

    public JumpExit( float jumpMultiplier )
    {
        this.jumpMultiplier = jumpMultiplier;
    }

    public override void Exit( Technique tech )
    {
        double charge = (tech.GetBlackboardData("charge") as double?) ?? 1.0;

        Transform rightFootAnchor = tech.owner.GetAnchor("FootR");
        Transform leftFootAnchor = tech.owner.GetAnchor("FootL");
        Vector3 emitterPosition = ( rightFootAnchor.position + leftFootAnchor.position ) / 2f;
        
        float mult = ((float)charge * jumpMultiplier) / 15f;
        tech.CreateEmitter("launch", emitterPosition, Vector3.SignedAngle(Vector3.up, tech.owner.aimDirection, Vector3.forward) )
            .Expand(mult)
            .Accelerate(mult);
    }
}

[System.Serializable]
public class JumpExitOptions : ExitTechStrategyOptions
{
    float jumpMultiplier;

    public override void InspectorDraw()
    {
        jumpMultiplier = EditorGUILayout.FloatField("Jump Multiplier", jumpMultiplier);
    }

    public override ExitTechStrategy GenerateStrategy()
    {
        return new JumpExit(jumpMultiplier);
    }
}
