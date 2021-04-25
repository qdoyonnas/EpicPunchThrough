using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimedChargeUpdateOptions : UpdateTechStrategyOptions
{
	public AnimationCurve chargeCurve = new AnimationCurve();
	public float duration;
	public double maxVF;
	public bool clampPositive;

	public override void InspectorDraw()
	{
		chargeCurve = EditorGUILayout.CurveField("Charge Curve", chargeCurve);
		duration = EditorGUILayout.FloatField("Duration", duration);
		maxVF = EditorGUILayout.DoubleField("Maximum VF", maxVF);
		clampPositive = EditorGUILayout.Toggle("Clamp Positive", clampPositive);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new TimedChargeUpdate(chargeCurve, duration, maxVF, clampPositive);
	}
}
