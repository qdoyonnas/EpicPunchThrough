using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Update/Charge")]
public class ChargeUpdateOptions : UpdateTechStrategyOptions
{
    public float frictionMultiplier;
    public double chargeRate;
    public double minimumCharge;
    public double maximumCharge;

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