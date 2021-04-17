using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/Techniques/Strategies/Exit/Flip Direction")]
public class FlipDirectionExitOptions : ExitTechStrategyOptions
{
	public override ExitTechStrategy GenerateStrategy()
	{
		return new FlipDirectionExit();
	}
}
