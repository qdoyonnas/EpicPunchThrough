using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndTechEventOptions : EventTechStrategyOptions
{
	public string eventKey;
	public bool blend = false;
	public bool skipExit = false;

	public override void InspectorDraw()
	{
		eventKey = EditorGUILayout.TextField("Event Key", eventKey);
		blend = EditorGUILayout.Toggle("Blend Transition", blend);
		skipExit = EditorGUILayout.Toggle("Skip Exit", skipExit);
	}

	public override EventTechStrategy GenerateStrategy()
	{
		return new EndTechEvent(eventKey, blend, skipExit);
	}
}