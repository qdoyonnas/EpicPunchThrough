using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChargeUpdateOptions : UpdateTechStrategyOptions
{
    public float frictionMultiplier;
    public double chargeRate;
    public long minimumCharge;
    public long maximumCharge;

    public override void InspectorDraw()
    {
        frictionMultiplier = EditorGUILayout.FloatField("Friction Multiplier", frictionMultiplier);
        chargeRate = EditorGUILayout.DoubleField("Charge Rate", chargeRate);
        minimumCharge = Math.Max(EditorGUILayout.LongField("Minimum Charge", minimumCharge), 0);
        maximumCharge = Math.Max(EditorGUILayout.LongField("Maximum Charge", maximumCharge), 0);
    }

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new ChargeUpdate(frictionMultiplier, chargeRate, (ulong)minimumCharge, (ulong)maximumCharge);
    }
}