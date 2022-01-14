using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTechStateChangeOptions : TechStateChangeStrategyOptions
{
	public override void InspectorDraw() {}

	public override TechStateChangeStrategy GenerateStrategy()
	{
		return new NoTechStateChange(inverseStates, validStates);
	}
}
