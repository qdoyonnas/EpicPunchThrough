using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LaunchExitOptions : ExitTechStrategyOptions
{
	public Direction direction;
	public float mult;

	public override void InspectorDraw()
	{
		direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
		mult = EditorGUILayout.FloatField("Force Multiplier", mult);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new LaunchExit(direction, mult);
	}
}
