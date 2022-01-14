using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JumpExitOptions : ExitTechStrategyOptions
{
    public float jumpMultiplier;
	public float minForce;
	public float maxForce;

    public override void InspectorDraw()
    {
		base.InspectorDraw();

        jumpMultiplier = EditorGUILayout.FloatField("Jump Multiplier", jumpMultiplier);
		minForce = EditorGUILayout.FloatField("Minimum Force", minForce);
		maxForce = EditorGUILayout.FloatField("Maximum Force", maxForce);
    }

    public override ExitTechStrategy GenerateStrategy()
    {
        return new JumpExit(inverseStates, validStates, jumpMultiplier, minForce, maxForce);
    }
}
