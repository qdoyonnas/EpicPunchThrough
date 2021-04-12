﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChargeUpdate : UpdateTechStrategy
{
    float frictionMultiplier;

    double chargeRate;
    double minimumCharge;
    double maximumCharge;

    public ChargeUpdate( float frictionMultiplier, double chargeRate, double minimumCharge, double maximumCharge )
    {
        this.frictionMultiplier = frictionMultiplier;

        this.chargeRate = chargeRate;
        this.minimumCharge = minimumCharge;
        this.maximumCharge = maximumCharge;
    }

    public override void Update( Technique tech, GameManager.UpdateData data, float value )
    {
        if( value > 0 ) {
            double charge = (tech.GetBlackboardData("charge") as double?) ?? 0.0;
            if( charge < minimumCharge ) {
                tech.SetBlackboardData("charge", minimumCharge);
            } else if( charge < maximumCharge ) {
                tech.SetBlackboardData("charge", charge + (chargeRate * data.deltaTime));
            }
        }

        if( tech.owner.slideParticle != null ) { tech.owner.slideParticle.enabled = true; }
        tech.owner.HandlePhysics( data, tech.owner.physicsBody.frictionCoefficients * frictionMultiplier );
    }
}

[System.Serializable]
public class ChargeUpdateOptions : UpdateTechStrategyOptions
{
    float frictionMultiplier;
    double chargeRate;
    double minimumCharge;
    double maximumCharge;

    public override void InspectorDraw()
    {
        frictionMultiplier = EditorGUILayout.FloatField("Friction Multiplier", frictionMultiplier);
        chargeRate = EditorGUILayout.DoubleField("Charge Rate", chargeRate);
        minimumCharge = EditorGUILayout.DoubleField("Minimum Charge", minimumCharge);
        maximumCharge = EditorGUILayout.DoubleField("Maximum Charge", maximumCharge);
    }

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new ChargeUpdate(frictionMultiplier, chargeRate, minimumCharge, maximumCharge);
    }
}
