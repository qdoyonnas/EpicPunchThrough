using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpdate : UpdateTechStrategy
{
    float frictionMultiplier;
    
    double chargeRate;
    ulong minimumCharge;
    ulong maximumCharge;

    public ChargeUpdate( float frictionMultiplier, double chargeRate, ulong minimumCharge, ulong maximumCharge )
    {
        this.frictionMultiplier = frictionMultiplier;

        this.chargeRate = chargeRate;
        this.minimumCharge = minimumCharge;
        this.maximumCharge = maximumCharge;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( value > 0 ) {
            if( tech.owner.chargingVF < minimumCharge ) {
                tech.owner.chargingVF = minimumCharge;
            } else if( tech.owner.chargingVF < maximumCharge ) {
                ulong chargeUp = (ulong)((chargeRate * tech.owner.chargeRate) * (double)data.deltaTime);
                Debug.Log(chargeUp);
                tech.owner.chargingVF += chargeUp;

                if( tech.owner.chargingVF > maximumCharge ) {
                    tech.owner.chargingVF = maximumCharge;
                }
            }
        }

        if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = true; }
        tech.owner.HandlePhysics( data, tech.owner.physicsBody.frictionCoefficients * frictionMultiplier );
    }
}
