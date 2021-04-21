using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	Text actionsText;

	private void Awake()
	{
		actionsText = transform.Find("Actions").GetComponent<Text>();
	}

	private void OnGUI()
	{
		if( AgentManager.Instance.playerAgent == null ) { return; }

		actionsText.text = string.Join(" : ", AgentManager.Instance.playerAgent.ActionSequence);
	}
}
