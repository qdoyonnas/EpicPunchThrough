using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LaunchEventOptions : EventTechStrategyOptions
{
	public string eventKey;
	public Direction direction;
	public float mult;
	public float minForce;
	public float maxForce;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		eventKey = EditorGUILayout.TextField("Event Key", eventKey);
		direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
		mult = EditorGUILayout.FloatField("Force Multiplier", mult);
		minForce = EditorGUILayout.FloatField("Minimum Force", minForce);
		maxForce = EditorGUILayout.FloatField("Maximum Force", maxForce);
	}

	public override EventTechStrategy GenerateStrategy()
	{
		return new LaunchEvent(inverseStates, validStates, eventKey, direction, mult, minForce, maxForce);
	}
}
