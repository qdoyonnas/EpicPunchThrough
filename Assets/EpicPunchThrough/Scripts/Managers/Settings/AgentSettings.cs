using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/Settings/AgentSettings", order = 1)]
public class AgentSettings : ScriptableObject
{
    [Header("Agent Settings")]
    public Agent.State initialAgentState = Agent.State.Grounded;
    public bool useController;

    [Header("Vital Force Settings")]
    [Min(0)] public long baseVitalForce;
    [Min(1)] public long defaultActiveVFFactor;
    [Min(1)] public long criticalSoulFactor;
    public double chargeRate;
    public double breakFactor;

    [Header("Animator Controllers")]
    public GameObject baseCharacterPrefab;
    public RuntimeAnimatorController baseCharacterController;
    public ParticleController baseParticleController;
    public int actionSequenceLength;

    [Header("Physic Settings")]
    public float autoStopSpeed;
    public Vector3 verticalBoundarySize;
    public Vector3 horizontalBoundarySize;
    public Agent.Action propPassThroughAction;
    public Agent.Action propCollideAction;

    [Header("Skins")]
    public CharacterSkin[] skins;

    [Header("Techniques")]
    public TechniqueSettings techniqueSettings;
}
