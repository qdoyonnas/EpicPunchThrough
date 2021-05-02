using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResistHitOptions : HitTechStrategyOptions
{
	public float breakResist = 0;

	public override void InspectorDraw()
	{
		EditorGUILayout.FloatField("Break Resist", breakResist);
	}

	public override HitTechStrategy GenerateStrategy()
	{
		return new ResistHit(breakResist);
	}
}
