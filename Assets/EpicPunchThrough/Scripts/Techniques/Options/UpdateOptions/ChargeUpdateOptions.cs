using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChargeUpdateOptions : UpdateTechStrategyOptions
{
	public double chargeRate;
	public long minimumCharge;
	public long maximumCharge;

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		chargeRate = EditorGUILayout.DoubleField("Charge Rate", chargeRate);
		minimumCharge = Math.Max(EditorGUILayout.LongField("Minimum Charge", minimumCharge), 0);
		maximumCharge = Math.Max(EditorGUILayout.LongField("Maximum Charge", maximumCharge), 0);
	}

	public override UpdateTechStrategy GenerateStrategy()
	{
		return new ChargeUpdate(inverseStates, validStates, chargeRate, (ulong)minimumCharge, (ulong)maximumCharge);
	}
}