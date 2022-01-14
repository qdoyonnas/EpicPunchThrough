using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoStateChangeOptions: StateChangeStrategyOptions
{
	public override void InspectorDraw() {}

	public override StateChangeStrategy GenerateStrategy()
	{
		return new NoStateChange( inverseStates, validStates );
	}
}