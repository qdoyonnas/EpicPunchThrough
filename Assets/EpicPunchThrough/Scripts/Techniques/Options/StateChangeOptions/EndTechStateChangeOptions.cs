using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndTechStateChangeOptions: StateChangeStrategyOptions
{
    public bool skipExit = true;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		skipExit = EditorGUILayout.Toggle("Skip Exit", skipExit);
	}

	public override StateChangeStrategy GenerateStrategy()
    {
        return new EndTechStateChange(inverseStates, validStates);
    }
}