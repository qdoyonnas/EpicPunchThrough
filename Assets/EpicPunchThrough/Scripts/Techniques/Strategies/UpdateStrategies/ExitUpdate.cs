using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUpdate : UpdateTechStrategy
{
	bool transition = false;

	public ExitUpdate( bool inverseStates, string[] states, bool transition )
		: base(inverseStates, states)
	{
		this.transition = transition;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( !ValidateState(tech) ) { return; }

		if( transition ) {
			tech.owner.TransitionTechnique(null, false);
		} else {
			tech.Exit();
		}
	}
}
