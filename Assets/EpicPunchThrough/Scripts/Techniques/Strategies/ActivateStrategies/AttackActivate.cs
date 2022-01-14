using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActivate : ActivateTechStrategy
{
    int attackVariations = 1;

    int variation = 0;

    public AttackActivate( bool inverseStates, string[] states, int variations )
		: base(inverseStates, states)
    {
        attackVariations = variations;
    }

	public override void Activate(Technique tech)
	{
		if( !ValidateState(tech) ) { return; }

        variation += 1;
		if( variation >= attackVariations )
		{
			variation = 0;
		}
		tech.owner.animator.SetInteger("Variation", variation);
	}
}
