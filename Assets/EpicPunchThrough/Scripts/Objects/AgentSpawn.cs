using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawn : MonoBehaviour
{
	public AgentManager.AgentType agentType = AgentManager.AgentType.fighter;
	public string agentName = "Fighter";
	public int agentTeam = 1;
	public bool isFacingRight = false;
	public string skinName = "";

	[Min(0)] public long vitalForce = 100000;
	[Min(1)] public long activeVitalForceFactor = 4;

	public void Awake()
	{
		if( vitalForce == 0 ) { vitalForce = AgentManager.Instance.settings.baseVitalForce; }
		if( agentType != AgentManager.AgentType.player ) {
			AgentManager.AgentSpawnData data = new AgentManager.AgentSpawnData(transform.position, 
														isFacingRight, 
														agentName, 
														agentType, 
														agentTeam, 
														skinName, 
														(ulong)vitalForce, 
														(ulong)activeVitalForceFactor);
			AgentManager.Instance.SpawnAgent(data);
		}
	}
}
