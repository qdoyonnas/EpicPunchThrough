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
        double charge = (tech.GetBlackboardData("charge") as double?) ?? 1.0;
		double mult = charge * (0.3 / speedMult);
		tech.owner.animator.SetFloat("AttackSpeed", (1/(float)mult));
		tech.owner.animator.SetTrigger("Attack");
	}
}
