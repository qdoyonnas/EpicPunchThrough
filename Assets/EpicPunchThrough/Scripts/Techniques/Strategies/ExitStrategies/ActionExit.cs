using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExit : ExitTechStrategy
{
	Agent.Action action;

	public ActionExit( bool inverseStates, string[] states, Agent.Action action )
		: base(inverseStates, states)
	{
		this.action = action;
	}

	public override void Exit(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		tech.owner.activeTechnique = null;
		tech.owner.PerformAction(action, 1);
	}
}

