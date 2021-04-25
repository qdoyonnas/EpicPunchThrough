using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChargeActivate : ActivateTechStrategy
{
	double vitalForce;
	bool isRatio;

	public SetChargeActivate( double vitalForce, bool isRatio )
	{
		this.vitalForce = vitalForce;
		this.isRatio = isRatio;
	}

	public override void Activate(Technique tech)
	{
		tech.owner.chargingVF = isRatio ? tech.owner.activeVF * vitalForce : vitalForce;
	}
}
