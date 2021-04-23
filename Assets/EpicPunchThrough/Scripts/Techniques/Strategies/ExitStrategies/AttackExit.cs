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
		double mult = (double)tech.owner.chargingVF * (0.3 / speedMult);
		double attackSpeed = 1/mult;
		tech.owner.animator.SetFloat("AttackSpeed", (float)attackSpeed);
		tech.owner.animator.SetTrigger("Attack");

		tech.SetBlackboardData("AttackSpeed", mult);
	}
}
