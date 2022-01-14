using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AgentUpdateOptions : UpdateTechStrategyOptions
{
	public float frictionMultiplier;
	public float gravityMultiplier;
	public bool slideParticles;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		frictionMultiplier = EditorGUILayout.FloatField("Friction Multiplier", frictionMultiplier);
		gravityMultiplier = EditorGUILayout.FloatField("Gravity Multiplier", gravityMultiplier);
		slideParticles = EditorGUILayout.Toggle("Slide Particles", slideParticles);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new AgentUpdate(inverseStates, validStates, frictionMultiplier, gravityMultiplier, slideParticles);
	}
}
