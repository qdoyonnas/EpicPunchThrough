using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionExitOptions : ExitTechStrategyOptions
{
	public Agent.Action action;

	public override void InspectorDraw()
	{
		base.InspectorDraw();
		
		action = (Agent.Action)EditorGUILayout.EnumPopup("Action", action);
	}

	public override ExitTechStrategy GenerateStrategy()
	{
		return new ActionExit(inverseStates, validStates, action);
	}
}