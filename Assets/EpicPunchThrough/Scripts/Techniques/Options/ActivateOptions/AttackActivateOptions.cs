using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackActivateOptions : ActivateTechStrategyOptions
{
    public int attackVariations = 1;

    public override void InspectorDraw()
    {
		base.InspectorDraw();

        attackVariations = EditorGUILayout.IntField("Attack Variations", attackVariations);
    }

    public override ActivateTechStrategy GenerateStrategy()
    {
        return new AttackActivate(inverseStates, validStates, attackVariations);
    }
}
