using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackActivate : ActivateTechStrategy
{
    int attackVariations = 1;

    int variation = 0;

    public AttackActivate( int variations )
    {
        attackVariations = variations;
    }

	public override void Activate(Technique tech)
	{
        variation += 1;
		if( variation >= attackVariations )
		{
			variation = 0;
		}
		tech.owner.animator.SetInteger("Variation", variation);
	}
}

[System.Serializable]
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
