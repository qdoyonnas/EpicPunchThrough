using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Activate/Attack")]
public class AttackActivateOptions : ActivateTechStrategyOptions
{
    public int attackVariations = 1;

    public override void InspectorDraw()
    {
        attackVariations = EditorGUILayout.IntField("Attack Variations", attackVariations);
    }

    public override ActivateTechStrategy GenerateStrategy()
    {
        return new AttackActivate(attackVariations);
    }
}
