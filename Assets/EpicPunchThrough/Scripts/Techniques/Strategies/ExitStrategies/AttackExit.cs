using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExit : ExitTechStrategy
{
	float speedMult = 1;

	public AttackExit(float mult)
	{
		speedMult = mult;
	}

	public override void Exit(Technique tech)
	{
		double chargeRatio = tech.GetBlackboardData("charge") as double? ?? 0.0f;
		chargeRatio = Math.Max(Math.Min(chargeRatio, 1), 0);

		double mult = speedMult / (0.8 * chargeRatio);
		mult = mult == 0 ? 0.01 : mult;
		tech.owner.animator.SetFloat("AttackSpeed", (float)mult);
		tech.owner.animator.SetTrigger("Attack");

		tech.SetBlackboardData("AttackSpeed", mult);
	}
}
