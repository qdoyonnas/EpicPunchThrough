using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	Text actionsText;
	Text chargeText;

	RectTransform VFBar;
	Image currentVF;
	Image activeVF;
	Text activeAmount;
	Image chargingVF;
	Image critical;

	private void Awake()
	{
		actionsText = transform.Find("Actions").GetComponent<Text>();
		chargeText = transform.Find("Charge").GetComponent<Text>();

		VFBar = transform.Find("VFBar").GetComponent<RectTransform>();
		currentVF = transform.Find("VFBar/Current").GetComponent<Image>();
		activeVF = transform.Find("VFBar/Active").GetComponent<Image>();
		activeAmount = activeVF.transform.Find("Amount").GetComponent<Text>();
		chargingVF = transform.Find("VFBar/Charging").GetComponent<Image>();
		critical = transform.Find("VFBar/Critical").GetComponent<Image>();
	}

	private void OnGUI()
	{
		if( AgentManager.Instance.playerAgent == null ) { return; }
		Agent player = AgentManager.Instance.playerAgent;

		actionsText.text = string.Join(" : ", player.ActionSequence);

		chargeText.text = $"Charge: {player.chargingVF}";

		float width = VFBar.rect.width;

		float currentX = -(width - (width * (float)player.VFRemaining)) - 1.5f;
		currentVF.rectTransform.anchoredPosition = new Vector2(currentX, currentVF.rectTransform.anchoredPosition.y);

		float activeX = -(width - (width * (1f / (float)player.activeFactor))) - 1.5f;
		activeVF.rectTransform.anchoredPosition = new Vector2(activeX, activeVF.rectTransform.anchoredPosition.y);
		activeAmount.text = Math.Floor(player.activeVF).ToString();

		float criticalX = -(width - (width * (1f / (float)player.criticalSoul))) - 1.5f;
		critical.rectTransform.anchoredPosition = new Vector2(criticalX, critical.rectTransform.anchoredPosition.y);

		float chargingX = -(width - (width * ((float)player.chargingVF / (float)player.maxVF)));
		chargingVF.rectTransform.anchoredPosition = new Vector2((width - Mathf.Abs(criticalX)) + chargingX, chargingVF.rectTransform.anchoredPosition.y);
	}
}
