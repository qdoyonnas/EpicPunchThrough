using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StaticLaunchExitOptions : ExitTechStrategyOptions
{
	public Direction direction;
	public AnimationCurve forceCurve = new AnimationCurve();
	public float force;
	public bool stayGrounded;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
		forceCurve = EditorGUILayout.CurveField("Force Curve", forceCurve);
		force = EditorGUILayout.FloatField("Force", force);
		stayGrounded = EditorGUILayout.Toggle("Stay grounded", stayGrounded);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new StaticLaunchExit(inverseStates, validStates, direction, forceCurve, force, stayGrounded);
	}
}
