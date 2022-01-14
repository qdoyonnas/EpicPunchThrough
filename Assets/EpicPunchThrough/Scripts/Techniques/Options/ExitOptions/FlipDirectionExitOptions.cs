using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDirectionExitOptions : ExitTechStrategyOptions
{
	public override ExitTechStrategy GenerateStrategy()
	{
		return new FlipDirectionExit(inverseStates, validStates);
	}
}
