using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetChargeActivateOptions : ActivateTechStrategyOptions
{
	public double vitalForce;
	public bool isRatio;

	public override void InspectorDraw()
	{
		EditorGUILayout.BeginHorizontal();
		vitalForce = EditorGUILayout.DoubleField("Vital Force", vitalForce);
		isRatio = EditorGUILayout.Toggle("Ratio", isRatio);
		EditorGUILayout.EndHorizontal();
	}

	public override ActivateTechStrategy GenerateStrategy()
	{
		return new SetChargeActivate(vitalForce, isRatio);
	}
}
