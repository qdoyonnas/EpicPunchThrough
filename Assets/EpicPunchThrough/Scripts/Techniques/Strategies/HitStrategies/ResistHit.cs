using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistHit : HitTechStrategy
{
	float breakResist;

	public ResistHit( float breakResist )
	{
		this.breakResist = breakResist;
	}

	public override void OnHit(Technique tech, Vector3 pushVector, double breakForce, Vector3 launchVector)
	{
		tech.owner.ProcessHit(pushVector, breakForce * (1 - breakResist), launchVector);
	}
}
