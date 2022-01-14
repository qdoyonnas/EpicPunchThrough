using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LaunchExitOptions : ExitTechStrategyOptions
{
	public Direction direction;
	public float mult;
	public float minForce;
	public float maxForce;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
		mult = EditorGUILayout.FloatField("Force Multiplier", mult);
		minForce = EditorGUILayout.FloatField("Minimum Force", minForce);
		maxForce = EditorGUILayout.FloatField("Maximum Force", maxForce);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new LaunchExit(inverseStates, validStates, direction, mult, minForce, maxForce);
	}
}
