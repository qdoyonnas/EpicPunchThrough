using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToAimActivateOptions : ActivateTechStrategyOptions
{
	public override ActivateTechStrategy GenerateStrategy()
	{
		return new FlipToAimActivate(inverseStates, validStates);
	}
}
