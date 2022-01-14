using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallSlideUpdateOptions : UpdateTechStrategyOptions
{
    public float frictionMultiplier;
    public float gravityMultiplier;

    public override void InspectorDraw()
    {
		base.InspectorDraw();

        frictionMultiplier = EditorGUILayout.FloatField("Friction Multiplier", frictionMultiplier);
        gravityMultiplier = EditorGUILayout.FloatField("Gravity Multiplier", gravityMultiplier);
    }

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new WallSlideUpdate( inverseStates, validStates, frictionMultiplier, gravityMultiplier);
    }
}