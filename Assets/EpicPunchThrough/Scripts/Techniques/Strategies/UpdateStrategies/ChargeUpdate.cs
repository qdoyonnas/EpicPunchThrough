using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpdate : UpdateTechStrategy
{
    double chargeRate;
    ulong minimumCharge;
    ulong maximumCharge;

    public ChargeUpdate( bool inverseStates, string[] states, double chargeRate, ulong minimumCharge, ulong maximumCharge )
        : base(inverseStates, states)
    {
        this.chargeRate = chargeRate;
        this.minimumCharge = minimumCharge;
        this.maximumCharge = maximumCharge;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( !ValidateState(tech) ) { return; }

        if( value > 0 ) {
            if( tech.owner.chargingVF < minimumCharge ) {
                tech.owner.chargingVF = minimumCharge;
            } else if( tech.owner.chargingVF < maximumCharge ) {
                double chargeUp = (chargeRate * tech.owner.chargeRate) * (double)data.deltaTime;

                tech.owner.chargingVF += chargeUp;

                if( tech.owner.chargingVF > maximumCharge ) {
                    tech.owner.chargingVF = maximumCharge;
                }
            }
        }

        tech.SetBlackboardData("charge", (double)tech.owner.chargingVF / (double)maximumCharge);
    }
}
