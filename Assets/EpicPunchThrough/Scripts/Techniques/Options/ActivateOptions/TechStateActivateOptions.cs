using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TechStateActivateOptions : ActivateTechStrategyOptions
{
	public string setState;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

        setState = EditorGUILayout.TextField("Set State", setState);
	}

	public override ActivateTechStrategy GenerateStrategy()
	{
		return new TechStateActivate(inverseStates, validStates, setState);
	}
}
