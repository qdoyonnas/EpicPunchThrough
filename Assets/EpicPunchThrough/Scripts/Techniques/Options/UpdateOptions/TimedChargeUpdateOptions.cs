using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimedChargeUpdateOptions : UpdateTechStrategyOptions
{
	public AnimationCurve chargeCurve = new AnimationCurve();
	public float duration = 0;
	public double maxVF = -1;
	public bool clampPositive = false;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		chargeCurve = EditorGUILayout.CurveField("Charge Curve", chargeCurve);
		duration = EditorGUILayout.FloatField("Duration", duration);
		maxVF = EditorGUILayout.DoubleField("Maximum VF", maxVF);
		clampPositive = EditorGUILayout.Toggle("Clamp Positive", clampPositive);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new TimedChargeUpdate(inverseStates, validStates, chargeCurve, duration, maxVF, clampPositive);
	}
}
