using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChargeActivate : ActivateTechStrategy
{
	double vitalForce;
	bool isRatio;

	public SetChargeActivate( bool inverseStates, string[] states, double vitalForce, bool isRatio )
		: base(inverseStates, states)
	{
		this.vitalForce = vitalForce;
		this.isRatio = isRatio;
	}

	public override void Activate(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		tech.owner.chargingVF = isRatio ? tech.owner.activeVF * vitalForce : vitalForce;
	}
}
