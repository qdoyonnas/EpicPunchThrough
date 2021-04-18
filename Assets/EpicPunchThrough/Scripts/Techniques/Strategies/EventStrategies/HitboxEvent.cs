using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxEvent : EventTechStrategy
{
	string eventKey;
	GameObject[] hitboxes;

	public HitboxEvent(string key, GameObject[] hitboxes)
	{
		this.eventKey = key;
		this.hitboxes = hitboxes;
	}

	public override void OnEvent(Technique tech, AnimationEvent e)
	{
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
			hitScript.Init(tech.owner, tech.owner.Team, attackSpeed);
		}
	}
}
