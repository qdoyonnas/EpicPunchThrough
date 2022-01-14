using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEventOptions: EventTechStrategyOptions
{
	public override void InspectorDraw() {}

	public override EventTechStrategy GenerateStrategy()
	{
		return new NoEvent(inverseStates, validStates);
	}
}