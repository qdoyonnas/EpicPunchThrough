using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDirectionExit : ExitTechStrategy
{
	public override void Exit(Technique tech)
	{
		tech.owner.isFacingRight = !tech.owner.isFacingRight;
	}
}

public class FlipDirectionExitOptions : ExitTechStrategyOptions
{
	public override ExitTechStrategy GenerateStrategy()
	{
		return new FlipDirectionExit();
	}
}
