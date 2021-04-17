using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExit : ExitTechStrategy
{
	Agent.Action action;

	public ActionExit(Agent.Action action)
	{
		this.action = action;
	}

	public override void Exit(Technique tech)
	{
		tech.owner.activeTechnique = null;
		tech.owner.PerformAction(action, 1);
	}
}

