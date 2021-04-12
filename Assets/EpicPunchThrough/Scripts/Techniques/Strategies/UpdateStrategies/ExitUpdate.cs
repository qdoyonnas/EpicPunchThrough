using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExitUpdate : UpdateTechStrategy
{
	bool transition = false;

	public ExitUpdate( bool transition )
	{
		this.transition = transition;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( transition ) {
			tech.owner.TransitionTechnique(null, false);
		} else {
			tech.Exit();
		}
	}
}

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
