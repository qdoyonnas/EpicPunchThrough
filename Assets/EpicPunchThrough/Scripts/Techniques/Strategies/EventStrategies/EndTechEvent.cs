using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndTechEvent : EventTechStrategy
{
	bool blend = false;
	bool skipExit = false;

	public EndTechEvent( bool blend, bool skipExit )
	{
		this.blend = blend;
		this.skipExit = skipExit;
	}

	public override void OnEvent( Technique tech, AnimationEvent e)
	{
		tech.owner.TransitionTechnique(null, blend, skipExit);
	}
}

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
