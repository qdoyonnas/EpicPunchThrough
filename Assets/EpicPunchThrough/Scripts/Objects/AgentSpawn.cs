using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawn : MonoBehaviour
{
	public AgentManager.AgentType agentType = AgentManager.AgentType.fighter;
	public string agentName = "Fighter";
	public int agentTeam = 1;
	public bool isFacingRight = false;

	public void Awake()
	{
		if( agentType != AgentManager.AgentType.player ) {
			AgentManager.AgentSpawnData data = new AgentManager.AgentSpawnData(transform.position, isFacingRight, agentName, agentType, agentTeam, "Vat Grunt");
			AgentManager.Instance.SpawnAgent(data);
		}
	}
}
