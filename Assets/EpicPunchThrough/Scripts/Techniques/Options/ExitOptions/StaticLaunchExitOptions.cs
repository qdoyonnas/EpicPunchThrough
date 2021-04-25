using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StaticLaunchExitOptions : ExitTechStrategyOptions
{
	public Direction direction;
	public AnimationCurve forceCurve = new AnimationCurve();
	public float force;

	public override void InspectorDraw()
	{
		direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
		forceCurve = EditorGUILayout.CurveField("Force Curve", forceCurve);
		force = EditorGUILayout.FloatField("Force", force);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new StaticLaunchExit(direction, forceCurve, force);
	}
}
