using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackValidate : ActionValidateTechStrategy
{
	float speedMult = 1;
    public ActionState[] actionStates;

	public AttackValidate(bool inverseStates, string[] states, float mult, params ActionState[] actionStates)
		: base(inverseStates, states)
	{
		speedMult = mult;
	}

	public override bool Validate(Technique tech, Agent.Action action, float value)
	{
		if( !ValidateState(tech) ) { return false; }

		foreach( ActionState actionState in actionStates ) {
			if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                CommitAttack(tech);
                return true;
            }
		}

		return false;
	}

	private void CommitAttack(Technique tech)
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
