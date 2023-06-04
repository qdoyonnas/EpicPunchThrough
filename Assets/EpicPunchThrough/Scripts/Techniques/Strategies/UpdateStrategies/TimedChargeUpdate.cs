using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedChargeUpdate : UpdateTechStrategy
{
	AnimationCurve chargeCurve;
	float duration;
	double maxVF = -1;
	bool clampPositive;

	float elapsedTime = 0f;

	public TimedChargeUpdate( bool inverseStates, string[] states, AnimationCurve curve, float duration, double maxVF, bool clampPositive )
		: base(inverseStates, states)
	{
		this.chargeCurve = curve;
		this.duration = duration;
		this.maxVF = maxVF;
		this.clampPositive = clampPositive;
	}

	public override void BeforeUpdate(Technique tech)
	{
		elapsedTime = 0f;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( !ValidateState(tech) ) { return; }

		double chargeRatio = tech.GetBlackboardData("charge") as double? ?? 0.0;
		if( value > 0 && (maxVF == -1 || tech.owner.chargingVF < maxVF) ) {
			elapsedTime += data.deltaTime;
			chargeRatio = elapsedTime / duration;

			float chargeValue = chargeCurve.Evaluate((float)chargeRatio);
			chargeValue = clampPositive ? Mathf.Max(chargeValue, 0) : chargeValue;
			tech.owner.chargingVF += tech.owner.chargeRate * chargeValue;

			if( maxVF != -1 && tech.owner.chargingVF > maxVF ) {
				tech.owner.chargingVF = maxVF;
			}

		}

		tech.SetBlackboardData("charge", chargeRatio);
	}
}
