using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public bool isStatic = false;
    public float lifeTime = 1;
    public bool hitsSelf = false;
    public bool hitsFriendlies = false;

    public Vector3 launchVector = Vector3.left;

    float inertiaCarry = 0;
    int team = -1;
    float lifeTimestamp = 0;
    Agent self;
    List<Agent> hitAgents = new List<Agent>();

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

	public void Init( Agent self, int team, double attackSpeed, Vector3 launchVector, float launchForce, float inertiaCarry )
    {
        this.self = self;
        if( !this.hitsFriendlies ) { this.team = team; }
        lifeTimestamp = Time.time + (lifeTime * (float)attackSpeed);
        if( launchVector.z == 0 ) { this.launchVector = launchVector; }
        this.launchVector *= launchForce;
        this.inertiaCarry = inertiaCarry;

        GameManager.Instance.fixedUpdated += OnUpdate;
    }

	private void OnTriggerEnter(Collider other)
	{
        Agent agent = other.GetComponentInParent<Agent>();
        if( agent == null || (!hitsSelf && agent == self) || agent.Team == team ) { return; }
        if( hitAgents.Contains(agent) ) { return; }

        if( agent.state != Agent.State.Flinched ) {
            hitAgents.Add(agent);
            agent.ReceiveHit(launchVector + (self.physicsBody.velocity * inertiaCarry));
        }
	}
}
