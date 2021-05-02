using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUpdate : UpdateTechStrategy
{
	bool transition = false;

	public ExitUpdate( bool transition )
	{
		this.transition = transition;
	}

	public override void Update(Technique tech, GameManager.UpdateData data, float value)
	{
		if( tech.state < Technique.State.Exit ) {
			if( transition ) {
				tech.owner.TransitionTechnique(null, false);
			} else {
				tech.Exit();
			}
		}
	}
}
