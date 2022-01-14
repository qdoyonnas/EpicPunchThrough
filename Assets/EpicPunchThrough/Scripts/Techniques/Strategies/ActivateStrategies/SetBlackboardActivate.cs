using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBlackboardActivate : ActivateTechStrategy
{
	Dictionary<string, object> data;

	public SetBlackboardActivate( bool inverseStates, string[] states, Dictionary<string, object> data )
		: base(inverseStates, states)
	{
		this.data = data;
	}

	public override void Activate(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

		foreach( KeyValuePair<string, object> entry in data ) {
			tech.SetBlackboardData(entry.Key, entry.Value);
		}
	}
}
