using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/CharacterSkin", order = 1)]
public class CharacterSkin : ScriptableObject
{
    public new string name;

	[Header("Body")]
	public Color headColor;
	public Color neckColor;
	public Color torsoColor;
	public Color lowerTorsoColor;
	public Color hipColor;

	[Header("Left Arm")]
	public Color leftShoulderColor;
	public Color leftUpperArmColor;
	public Color leftElbowColor;
	public Color leftLowerArmColor;
	public Color leftHandColor;

	[Header("Right Arm")]
	public Color rightShoulderColor;
	public Color rightUpperArmColor;
	public Color rightElbowColor;
	public Color rightLowerArmColor;
	public Color rightHandColor;

	[Header("Left Leg")]
	public Color leftUpperLegColor;
	public Color leftLowerLegColor;
	public Color leftKneeColor;
	public Color leftFootColor;

	[Header("Right Leg")]
	public Color rightUpperLegColor;
	public Color rightLowerLegColor;
	public Color rightKneeColor;
	public Color rightFootColor;
}
