using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionExit : ExitTechStrategy
{
	Agent.Action action;

	public ActionExit(Agent.Action action)
	{
		this.action = action;
	}

	public override void Exit(Technique tech)
	{
		tech.owner.activeTechnique = null;
		tech.owner.PerformAction(action, 1);
	}
}

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