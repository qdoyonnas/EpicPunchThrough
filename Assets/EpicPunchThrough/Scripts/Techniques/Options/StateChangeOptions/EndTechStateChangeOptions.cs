using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndTechStateChangeOptions: StateChangeStrategyOptions
{
    public bool abortTechnique = true;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		abortTechnique = EditorGUILayout.Toggle("Abort Technique", abortTechnique);
	}

	public override StateChangeStrategy GenerateStrategy()
    {
        return new EndTechStateChange(inverseStates, validStates, abortTechnique);
    }
}