using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public bool isStatic = false;
    public float lifeTime = 1;
    public bool hitsSelf = false;
    public bool hitsFriendlies = false;

    public float pushForce;
    public double breakForce;
    public float launchForce;
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

	public void Init( Agent self, int team, double attackSpeed, Vector3 launchVector, float pushForce, double breakForce, float launchForce, float inertiaCarry )
    {
        this.self = self;
        if( !this.hitsFriendlies ) { this.team = team; }
        lifeTimestamp = Time.time + (lifeTime / (float)attackSpeed);
        this.launchVector = launchVector;
        this.pushForce = pushForce;
        this.breakForce = breakForce;
        this.launchForce = launchForce;
        this.inertiaCarry = inertiaCarry;

        GameManager.Instance.fixedUpdated += OnUpdate;
    }

	private void OnTriggerEnter(Collider other)
	{
        Agent agent = other.GetComponentInParent<Agent>();
        if( agent == null || (!hitsSelf && agent == self) || agent.Team == team ) { return; }
        if( hitAgents.Contains(agent) ) { return; }

        switch( agent.state ) {
            case Agent.State.Flinched:
            case Agent.State.Launched:
                break;
            default:
                hitAgents.Add(agent);

                Vector3 totalPushVector = launchVector * (self.physicsBody.velocity.magnitude * pushForce);
                Vector3 totalLaunchVector = (launchVector * launchForce) + (self.physicsBody.velocity * inertiaCarry);

                self.physicsBody.velocity = Vector3.zero;
                agent.ReceiveHit(totalPushVector, breakForce, totalLaunchVector);
                break;
        }
	}
}
