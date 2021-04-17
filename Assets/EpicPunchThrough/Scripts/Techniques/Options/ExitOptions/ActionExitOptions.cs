using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Exit/Action")]
public class ActionExitOptions : ExitTechStrategyOptions
{
	public Agent.Action action;

	public override void InspectorDraw()
	{
		action = (Agent.Action)EditorGUILayout.EnumPopup("Action", action);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new ActionExit(action);
	}
}