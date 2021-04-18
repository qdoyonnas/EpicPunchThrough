using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExitUpdateOptions : UpdateTechStrategyOptions
{
	public bool transition = false;

	public override void InspectorDraw()
	{
		transition = EditorGUILayout.Toggle("End Technique", transition);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new ExitUpdate(transition);
	}
}