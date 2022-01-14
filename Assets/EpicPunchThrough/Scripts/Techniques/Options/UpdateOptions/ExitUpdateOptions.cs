using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExitUpdateOptions : UpdateTechStrategyOptions
{
	public bool transition = false;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		transition = EditorGUILayout.Toggle("End Technique", transition);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new ExitUpdate(inverseStates, validStates, transition);
	}
}