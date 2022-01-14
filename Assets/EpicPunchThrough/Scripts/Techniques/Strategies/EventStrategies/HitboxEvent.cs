using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxEvent : EventTechStrategy
{
	string eventKey;
	Direction launchDirection;
	float pushMult;
	float breakMult;
	float launchMult;
	float inertiaCarry;
	GameObject[] hitboxes;

	public HitboxEvent( bool inverseStates, string[] states, string key, Direction launchDirection, float pushForce, float breakForce, float launchForce, float inertia, GameObject[] hitboxes )
		: base(inverseStates, states)
	{
		this.launchDirection = launchDirection;
		this.pushMult = pushForce;
		this.breakMult = breakForce;
		this.launchMult = launchForce;
		this.eventKey = key;
		this.hitboxes = hitboxes;
		this.inertiaCarry = inertia;
	}

	public override void OnEvent(Technique tech, AnimationEvent e)
	{
		if( !ValidateState(tech) ) { return; }

		if( !string.IsNullOrEmpty(eventKey)
			&& eventKey != e.stringParameter ) {
			return;
		}

		GameObject hitbox = hitboxes[0];
		if( e.intParameter < hitboxes.Length ) { 
			hitbox = hitboxes[e.intParameter];
		}

		GameObject hit = GameObject.Instantiate(hitbox, tech.owner.transform.position, tech.owner.transform.rotation);
		if( tech.owner.isFacingRight ) {
			hit.transform.localScale = new Vector3(-1, 1, 1);
		}
		Hitbox hitScript = hit.GetComponent<Hitbox>();
		if( hitScript != null ) {
			if( !hitScript.isStatic ) {
				hit.transform.parent = tech.owner.transform;
			}

			double attackSpeed = tech.GetBlackboardData("AttackSpeed") as double? ?? 1.0;

			Vector3 launchVectorOverride = Utilities.GetDirectionVector(tech.owner, launchDirection);
			if( launchDirection == Direction.None ) { launchVectorOverride = Vector3.back; }

			double breakForce = tech.owner.chargingVF * breakMult;
			float launchForce = Utilities.VFToForce(tech.owner.chargingVF * launchMult);

			hitScript.Init(tech.owner, tech.owner.Team, attackSpeed, launchVectorOverride, pushMult, breakForce, launchForce, inertiaCarry);
		}
	}
}
