using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Exit/Jump")]
public class JumpExitOptions : ExitTechStrategyOptions
{
    public float jumpMultiplier;

    public override void InspectorDraw()
    {
        jumpMultiplier = EditorGUILayout.FloatField("Jump Multiplier", jumpMultiplier);
    }

    public override ExitTechStrategy GenerateStrategy()
    {
        return new JumpExit(jumpMultiplier);
    }
}
