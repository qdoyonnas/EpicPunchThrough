using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStateActivate : ActivateTechStrategy
{
	string setState = string.Empty;

	public TechStateActivate(bool inverseStates, string[] states, string setState)
		:base(inverseStates, states)
	{
		this.setState = setState;
	}

	public override void Activate(Technique tech)
	{
		tech.state = setState;
	}
}
