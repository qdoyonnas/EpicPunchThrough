using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStateValidate : ActionValidateTechStrategy
{
	public string setState;
    public ActionState[] actionStates;

	public TechStateValidate(bool inverseStates, string[] states, string setState, params ActionState[] actionStates)
		: base(inverseStates, states)
	{
		this.setState = setState;
		this.actionStates = actionStates;
	}

	public override bool Validate(Technique tech, Agent.Action action, float value)
	{
		if( !ValidateState(tech) ) { return false; }

		foreach( ActionState actionState in actionStates ) {
            if( actionState.action == action && actionState.state == (Mathf.Abs(value) > 0)  ) {
                tech.state = setState;
                return true;
            }
        }

		return false;
	}
}
