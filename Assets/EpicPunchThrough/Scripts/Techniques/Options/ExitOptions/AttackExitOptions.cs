using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackExitOptions : ExitTechStrategyOptions
{
	public float speedMult = 1;

	public override void InspectorDraw()
	{
		speedMult = EditorGUILayout.FloatField("Speed", speedMult);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new AttackExit(speedMult);
	}
}
