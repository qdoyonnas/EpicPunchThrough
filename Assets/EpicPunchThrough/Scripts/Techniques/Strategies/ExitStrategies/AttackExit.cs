using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackExit : ExitTechStrategy
{
    float damageMultiplier;
    float forceMultiplier;

    public AttackExit( float damageMultiplier, float forceMultiplier )
    {
        this.damageMultiplier = damageMultiplier;
        this.forceMultiplier = forceMultiplier;
    }

    public override void Exit( Technique tech )
    {
        tech.owner.animator.SetTrigger("Attack");

        float attackSpeed;
        double charge = (tech.GetBlackboardData("charge") as double?) ?? 1.0;
        float mult = (float)(charge / 10.0);
        attackSpeed = 1 / mult;
        tech.owner.animator.SetFloat("AttackSpeed", attackSpeed);
    }
}

[System.Serializable]
public class AttackExitOptions : ExitTechStrategyOptions
{
    public float damageMultiplier;
    public float forceMultiplier;

    public override void InspectorDraw()
    {
        damageMultiplier = EditorGUILayout.FloatField("Damage Multiplier", damageMultiplier);
        forceMultiplier = EditorGUILayout.FloatField("Force Multiplier", forceMultiplier);
    }

    public override ExitTechStrategy GenerateStrategy()
    {
        return new AttackExit(damageMultiplier, forceMultiplier);
    }
}
