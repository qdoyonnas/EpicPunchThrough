using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Event/End Tech")]
public class EndTechEventOptions : EventTechStrategyOptions
{
	public bool blend = false;
	public bool skipExit = false;

	public override void InspectorDraw()
	{
		blend = EditorGUILayout.Toggle("Blend Transition", blend);
		skipExit = EditorGUILayout.Toggle("Skip Exit", skipExit);
	}

	public override EventTechStrategy GenerateStrategy()
	{
		return new EndTechEvent(blend, skipExit);
	}
}