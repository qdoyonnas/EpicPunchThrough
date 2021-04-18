using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public bool isStatic = false;
    public float lifeTime = 1;
    public bool hitsSelf = false;
    public bool hitsFriendlies = false;

    int team = -1;
    float lifeTimestamp = 0;
    Agent self;

    public void OnUpdate( GameManager.UpdateData data )
    {
        if( Time.time >= lifeTimestamp ) {
            Destroy(gameObject);
        }
    }

	private void OnDestroy()
	{
		GameManager.Instance.fixedUpdated -= OnUpdate;
	}

	public void Init(Agent self, int team, double attackSpeed)
    {
        this.self = self;
        if( !this.hitsFriendlies ) { this.team = team; }
        lifeTimestamp = Time.time + (lifeTime * (float)attackSpeed);
        GameManager.Instance.fixedUpdated += OnUpdate;
    }

	private void OnTriggerEnter(Collider other)
	{
        Agent agent = other.GetComponentInParent<Agent>();
        if( agent == null || (!hitsSelf && agent == self) || agent.Team == team ) { return; }

		agent.ReceiveHit();
	}
}
