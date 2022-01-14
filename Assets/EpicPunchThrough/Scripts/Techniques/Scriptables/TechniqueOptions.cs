using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Project/Techniques/Technique")]
public class TechniqueOptions : ScriptableObject
{
    public string techniqueName;
    public RuntimeAnimatorController animatorController;
    public ParticleController particleController;
    
    public Agent.State[] states;
    public Agent.Action[] actionSequence;

    public bool consumeVF = true;

    [SerializeField] public TriggerTechStrategyOptions[] triggerStrategies;
    [SerializeField] public ActivateTechStrategyOptions[] activateStrategies;
    [SerializeField] public TechStateChangeStrategyOptions[] techStateStrategies;
    [SerializeField] public StateChangeStrategyOptions[] stateStrategies;
    [SerializeField] public ActionValidateTechStrategyOptions[] actionValidateStrategies;
    [SerializeField] public UpdateTechStrategyOptions[] updateStrategies;
    [SerializeField] public EventTechStrategyOptions[] eventStrategies;
    [SerializeField] public HitTechStrategyOptions[] hitStrategies;
    [SerializeField] public ExitTechStrategyOptions[] exitStrategies;

	public void SetDirtyRecursive()
    {
        EditorUtility.SetDirty(this);
        foreach( TriggerTechStrategyOptions stratOptions in triggerStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( ActivateTechStrategyOptions stratOptions in activateStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( TechStateChangeStrategyOptions stratOptions in techStateStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( StateChangeStrategyOptions stratOptions in stateStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( ActionValidateTechStrategyOptions stratOptions in actionValidateStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( UpdateTechStrategyOptions stratOptions in updateStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( EventTechStrategyOptions stratOptions in eventStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( HitTechStrategyOptions stratOptions in hitStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
        foreach( ExitTechStrategyOptions stratOptions in exitStrategies ) {
            EditorUtility.SetDirty(stratOptions);
        }
    }
}