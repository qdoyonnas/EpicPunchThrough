using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AgentUpdateOptions : UpdateTechStrategyOptions
{
	public float frictionMultiplier;

	public override void InspectorDraw()
	{
		frictionMultiplier = EditorGUILayout.FloatField("Friction Multiplier", frictionMultiplier);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new AgentUpdate(frictionMultiplier);
	}
}
