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

	float borderWidth = 3f;

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

		float currentX = -(1f - (float)player.VFRemaining) * width - borderWidth;
		SetRight(currentVF.rectTransform, currentX);

		float activeX = -(1f - (1f / (float)player.activeFactor)) * width - borderWidth;
		SetRight(activeVF.rectTransform, activeX);
		activeAmount.text = (player.activeVF / 100).ToString();

		float criticalX = -(1f - (1f / (float)player.criticalSoul)) * width - borderWidth;
		SetRight(critical.rectTransform, criticalX);

		criticalX = (1f / (float)player.criticalSoul) * width - borderWidth;
		float chargingX = -((1f - ((float)player.chargingVF / (float)player.maxVF)) * width) + criticalX;
		SetLeft(chargingVF.rectTransform, criticalX);
		SetRight(chargingVF.rectTransform, chargingX);
	}

	private void SetLeft(RectTransform rect, float value)
	{
		rect.offsetMin = new Vector2(value, rect.offsetMin.y);
	}
	private void SetRight(RectTransform rect, float value)
	{
		rect.offsetMax = new Vector2(value, rect.offsetMax.y);
	}
}
