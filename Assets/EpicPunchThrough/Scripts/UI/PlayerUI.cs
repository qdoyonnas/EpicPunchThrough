using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	Text actionsText;

	RectTransform VFBar;
	Image currentVF;
	Image activeVF;
	Image chargingVF;
	Image critical;

	private void Awake()
	{
		actionsText = transform.Find("Actions").GetComponent<Text>();

		VFBar = transform.Find("VFBar").GetComponent<RectTransform>();
		currentVF = transform.Find("VFBar/Current").GetComponent<Image>();
		activeVF = transform.Find("VFBar/Active").GetComponent<Image>();
		chargingVF = transform.Find("VFBar/Charging").GetComponent<Image>();
		critical = transform.Find("VFBar/Critical").GetComponent<Image>();
	}

	private void OnGUI()
	{
		if( AgentManager.Instance.playerAgent == null ) { return; }
		Agent player = AgentManager.Instance.playerAgent;

		actionsText.text = string.Join(" : ", player.ActionSequence);

		float width = VFBar.rect.width;

		float currentX = -(width - (width * (float)player.VFRemaining)) - 1.5f;
		currentVF.rectTransform.anchoredPosition = new Vector2(currentX, currentVF.rectTransform.anchoredPosition.y);

		float activeX = -(width - (width * (1f / (float)player.activeFactor))) - 1.5f;
		activeVF.rectTransform.anchoredPosition = new Vector2(activeX, activeVF.rectTransform.anchoredPosition.y);

		float criticalX = -(width - (width * (1f / (float)player.criticalSoul))) - 1.5f;
		critical.rectTransform.anchoredPosition = new Vector2(criticalX, critical.rectTransform.anchoredPosition.y);

		float chargingX = -(width - (width * ((float)player.chargingVF / (float)player.maxVF))) - 1.5f;
		chargingVF.rectTransform.anchoredPosition = new Vector2(critical.rectTransform.anchorMax.x + chargingX, chargingVF.rectTransform.anchoredPosition.y);
	}
}
