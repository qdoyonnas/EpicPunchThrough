﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechniqueGenerator
{
    #region Static

    private static TechniqueGenerator instance;
    public static TechniqueGenerator Instance
    {
        get {
            if( instance == null ) {
                instance = new TechniqueGenerator();
            }
            return instance;
        }
    }

    #endregion
    
    public string baseAnimatorControllerPath = "Base/BaseCharacter";

	public class StrategyComparer : IComparer<TechStrategyOptions> {
		public int Compare(TechStrategyOptions strategyX, TechStrategyOptions strategyY)
		{
            return strategyX.order.CompareTo(strategyY.order);
		}
	}
    IComparer<TechStrategyOptions> strategyComparer = new StrategyComparer();

	public void GenerateTechnique( Agent agent, TechniqueOptions options )
    {
        if( options == null ) { return; }
        RuntimeAnimatorController animController = options.animatorController;
        if( animController == null ) { return; }
        ParticleController particleController = options.particleController;

        Technique.TechTrigger techTrigger = GenerateTechTrigger(options);

        TriggerTechStrategyOptions[] triggerStrategyOptions = options.triggerStrategies == null ? new TriggerTechStrategyOptions[] { ScriptableObject.CreateInstance<NoInterruptTriggerOptions>() } : options.triggerStrategies;
        ActivateTechStrategyOptions[] activateStrategyOptions = options.activateStrategies == null ? new ActivateTechStrategyOptions[] { ScriptableObject.CreateInstance<NoActivateOptions>() } : options.activateStrategies;
        TechStateChangeStrategyOptions[] techStateStrategyOptions = options.techStateStrategies == null ? new TechStateChangeStrategyOptions[] { ScriptableObject.CreateInstance<NoTechStateChangeOptions>() } : options.techStateStrategies;
        StateChangeStrategyOptions[] stateStrategyOptions = options.stateStrategies == null ? new StateChangeStrategyOptions[] { ScriptableObject.CreateInstance<EndTechStateChangeOptions>() } : options.stateStrategies;
        ActionValidateTechStrategyOptions[] actionValidateStrategyOptions = options.actionValidateStrategies == null ? new ActionValidateTechStrategyOptions[] { ScriptableObject.CreateInstance<NoValidateOptions>() } : options.actionValidateStrategies;
        UpdateTechStrategyOptions[] updateStrategyOptions = options.updateStrategies == null ? new UpdateTechStrategyOptions[] { ScriptableObject.CreateInstance<NoUpdateOptions>() } : options.updateStrategies;
        EventTechStrategyOptions[] eventStrategyOptions = options.eventStrategies == null ? new EventTechStrategyOptions[]{ ScriptableObject.CreateInstance<NoEventOptions>() } : options.eventStrategies;
        HitTechStrategyOptions[] hitStrategyOptions = options.hitStrategies == null ? new HitTechStrategyOptions[]{ ScriptableObject.CreateInstance<DefaultHitOptions>() } : options.hitStrategies;
        ExitTechStrategyOptions[] exitStrategyOptions = options.exitStrategies == null ? new ExitTechStrategyOptions[] { ScriptableObject.CreateInstance<NoExitOptions>() } : options.exitStrategies;

        TriggerTechStrategy[] triggerStrategies = new TriggerTechStrategy[triggerStrategyOptions.Length];
        Array.Sort(triggerStrategyOptions, strategyComparer);
        for( int i = 0; i < triggerStrategyOptions.Length; i++ ) {
            triggerStrategies[i] = triggerStrategyOptions[i].GenerateStrategy();
        }
        ActivateTechStrategy[] activateStrategies = new ActivateTechStrategy[activateStrategyOptions.Length];
        Array.Sort(activateStrategyOptions, strategyComparer);
        for( int i = 0; i < activateStrategyOptions.Length; i++ ) {
            activateStrategies[i] = activateStrategyOptions[i].GenerateStrategy();
        }
        TechStateChangeStrategy[] techStateStrategies = new TechStateChangeStrategy[techStateStrategyOptions.Length];
        Array.Sort(techStateStrategyOptions, strategyComparer);
        for( int i = 0; i < techStateStrategyOptions.Length; i++ ) {
            techStateStrategies[i] = techStateStrategyOptions[i].GenerateStrategy();
        }
        StateChangeStrategy[] stateStrategies = new StateChangeStrategy[stateStrategyOptions.Length];
        Array.Sort(stateStrategyOptions, strategyComparer);
        for( int i = 0; i < stateStrategyOptions.Length; i++ ) {
            stateStrategies[i] = stateStrategyOptions[i].GenerateStrategy();
        }
        ActionValidateTechStrategy[] actionValidateStrategies = new ActionValidateTechStrategy[actionValidateStrategyOptions.Length];
        Array.Sort(actionValidateStrategyOptions, strategyComparer);
        for( int i = 0; i < actionValidateStrategyOptions.Length; i++ ) {
            actionValidateStrategies[i] = actionValidateStrategyOptions[i].GenerateStrategy();
        }
        UpdateTechStrategy[] updateStrategies = new UpdateTechStrategy[updateStrategyOptions.Length];
        Array.Sort(updateStrategyOptions, strategyComparer);
        for( int i = 0; i < updateStrategyOptions.Length; i++ ) {
            updateStrategies[i] = updateStrategyOptions[i].GenerateStrategy();
        }
        EventTechStrategy[] eventStrategies = new EventTechStrategy[eventStrategyOptions.Length];
        Array.Sort(eventStrategyOptions, strategyComparer);
        for( int i = 0; i < eventStrategyOptions.Length; i++ ) {
            eventStrategies[i] = eventStrategyOptions[i].GenerateStrategy();
        }
        HitTechStrategy[] hitStrategies = new HitTechStrategy[hitStrategyOptions.Length];
        Array.Sort(hitStrategyOptions, strategyComparer);
        for( int i = 0; i < hitStrategyOptions.Length; i++ ) {
            hitStrategies[i] = hitStrategyOptions[i].GenerateStrategy();
        }
        ExitTechStrategy[] exitStrategies = new ExitTechStrategy[exitStrategyOptions.Length];
        Array.Sort(exitStrategyOptions, strategyComparer);
        for( int i = 0; i < exitStrategyOptions.Length; i++ ) {
            exitStrategies[i] = exitStrategyOptions[i].GenerateStrategy();
        }

        Technique tech = new Technique(agent, options.techniqueName,  animController, particleController, techTrigger, options.consumeVF,
                                triggerStrategies, activateStrategies, techStateStrategies, stateStrategies, actionValidateStrategies, 
                                updateStrategies, eventStrategies, hitStrategies, exitStrategies);
        agent.AddTechnique(tech);
    }

    Technique.TechTrigger GenerateTechTrigger( TechniqueOptions options )
    {
        Technique.TechTrigger techTrigger = new Technique.TechTrigger();
        if( options.actionSequence == null || options.actionSequence.Length == 0 ) {
            Debug.LogError("Technique generated with no trigger actions. Probably undersired behaviour.");
            techTrigger = new Technique.TechTrigger(options.states);
        } else {
            techTrigger = new Technique.TechTrigger(options.states, options.actionSequence);
        }

        return techTrigger;
    }

    public void AddTechniqueSet(Agent agent, TechniqueSet set)
    {
        foreach( TechniqueOptions options in set.techniques ) {
            GenerateTechnique(agent, options);
        }
    }
}