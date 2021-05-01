using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Technique
{
    public string name;
    protected Agent _owner;
    public Agent owner {
        get {
            return _owner;
        }
    }
    protected RuntimeAnimatorController _animatorController;
    public RuntimeAnimatorController animatorController {
        get {
            return _animatorController;
        }
    }

    protected ParticleController _particleController;
    public ParticleController particleController {
        get {
            return _particleController;
        }
    }

    public enum State {
        Trigger = 0,
        Activate = 1,
        Update = 2,
        Exit = 3,
        Inactive = 4
    }
    protected State _state = State.Inactive;
    public State state {
        get {
            return _state;
        }
    }
    protected Dictionary<string, object> blackboard = new Dictionary<string, object>();

    #region TechTrigger

    public struct TechTrigger {
        public readonly Agent.State[] states;
        public readonly Agent.Action[] sequence;

        public TechTrigger(Agent.State[] states, params Agent.Action[] actions)
        {
            this.states = states;
            sequence = actions;
        }
    }
    protected TechTrigger _techTrigger;
    public TechTrigger techTrigger {
        get {
            return _techTrigger;
        }
    }

    #endregion

    #region Strategies

    protected TriggerTechStrategy[] triggerStrategies;
    protected ActivateTechStrategy[] activateStrategies;
    protected StateChangeStrategy[] stateStrategies;
    protected ActionValidateTechStrategy[] validateStrategies;
    protected UpdateTechStrategy[] updateStrategies;
    protected EventTechStrategy[] eventStrategies;
    protected HitTechStrategy[] hitStrategies;
    protected ExitTechStrategy[] exitStrategies;

    #endregion
    
    public Technique( Agent owner, string name, RuntimeAnimatorController animCtrl, ParticleController particleController, TechTrigger techTrgr, 
        TriggerTechStrategy[] triggerStrategy, ActivateTechStrategy[] activateStrategy, StateChangeStrategy[] stateStrategy,
        ActionValidateTechStrategy[] validateStrategy, UpdateTechStrategy[] updateStrategy, EventTechStrategy[] eventStrategy, 
        HitTechStrategy[] hitStrategy, ExitTechStrategy[] exitStrategy )
    {
        if( owner == null || animCtrl == null ) { 
            Debug.LogError("Technique generated with empty arguments");
            return;
        }

        this.name = name;

        this._owner = owner;
        _techTrigger = techTrgr;
        _animatorController = animCtrl;
        _particleController = particleController;

        this.triggerStrategies = triggerStrategy;
        this.activateStrategies = activateStrategy;
        this.stateStrategies = stateStrategy;
        this.validateStrategies = validateStrategy;
        this.updateStrategies = updateStrategy;
        this.eventStrategies = eventStrategy;
        this.hitStrategies = hitStrategy;
        this.exitStrategies = exitStrategy;

        owner.SubscribeToActionEvent(techTrigger.sequence[techTrigger.sequence.Length-1], OnTrigger);
    }

    #region Behaviour Methods

    public virtual void OnTrigger( Agent.ActionEventArgs e )
    {
        if( Array.IndexOf(techTrigger.states, Agent.State.Any) == -1
                && Array.IndexOf(techTrigger.states, owner.state) == -1 ) 
        { return; }
        

        if( techTrigger.sequence.Length > 1 ) {
            if( owner.ActionSequence.Length < techTrigger.sequence.Length - 1 ) { return; }
            for( int i = 1; i <= techTrigger.sequence.Length - 1; i++ ) {
                if( techTrigger.sequence[(techTrigger.sequence.Length - 1) - i] != owner.ActionSequence[owner.ActionSequence.Length - i] ) {
                    return;
                }
            }
        }

        _state = State.Trigger;
        foreach( TriggerTechStrategy strategy in triggerStrategies ) {
            if( !strategy.Trigger(this, e.value) ) { return; }
        }

        e.activated = true;
        owner.AddActivatingTechnique(this);
    }
    public virtual void Activate()
    {
        _state = State.Activate;

        foreach( ActivateTechStrategy strategy in activateStrategies ) {
            strategy.Activate(this);
        }

        _state = State.Update;
        foreach( UpdateTechStrategy strategy in updateStrategies ) {
            strategy.BeforeUpdate(this);
        }
    }
    public virtual void OnStateChange( Agent.State previousState, Agent.State newState )
    {
        foreach( StateChangeStrategy strategy in stateStrategies ) {
            strategy.OnStateChange(this, previousState, newState);
        }
    }
    /// <summary>
    /// Returns whether the technique allows the given action to take place during the techniques execution.
    /// May cause additional behaviour to take place at the same time.
    /// </summary>
    /// <param name="action">The action in question</param>
    /// <returns>Boolean indicating whether the action is allowed during the technique</returns>
    public virtual bool ValidateAction(Agent.Action action, float value)
    {
        bool valid = true;
        foreach( ActionValidateTechStrategy strategy in validateStrategies ) {
            if( !strategy.Validate(this, action, value) ) {
                valid = false;
            }
        }

        return valid;
    }
    public virtual void Update( GameManager.UpdateData data, float value )
    {
        foreach( UpdateTechStrategy strategy in updateStrategies ) {
            strategy.Update( this, data, value );
        }
    }
    public virtual void OnAnimationEvent( AnimationEvent e )
    {
        foreach( EventTechStrategy strategy in eventStrategies ) {
            strategy.OnEvent(this, e);
        }
    }
    public virtual void OnHit(Vector3 pushVector, double breakForce, Vector3 launchVector)
    {
        foreach( HitTechStrategy strategy in hitStrategies ) {
            strategy.OnHit(this, pushVector, breakForce, launchVector);
        }
    }
    public virtual void Exit()
    {
        foreach( UpdateTechStrategy strategy in updateStrategies ) {
            strategy.AfterUpdate(this);
        }

        _state = State.Exit;
        foreach( ExitTechStrategy strategy in exitStrategies ) {
            strategy.Exit(this);
        }
    }
    public virtual void DeActivate()
    {
        _state = State.Inactive;
        ClearBlackboardData();
        owner.chargingVF = 0;
    }

    #endregion

    public void SetBlackboardData( string key, object data )
    {
        string l_key = key.ToLower();
        blackboard[l_key] = data;
    }
    public object GetBlackboardData( string key )
    {
        string l_key = key.ToLower();

        if( !blackboard.ContainsKey(l_key) ) {
            SetBlackboardData( l_key, null );
        }

        return blackboard[l_key];
    }
    public void ClearBlackboardData()
    {
        blackboard.Clear();
    }
    public void ClearBlackboardData( string key )
    {
        string l_key = key.ToLower();
        blackboard.Remove(l_key);
    }

    #region Utility

    public override string ToString()
    {
        return string.Format("Tech: {0}: {1}", owner.gameObject.name, name);
    }

    public bool IsValid()
    {
        return owner != null;
    }

    public virtual ParticleEmitter CreateEmitter( string particleName, Vector3 position, float angle, Transform parent = null)
    {
        if( particleController == null ) { return null; }

        ParticleEmitter emitter = ParticleManager.Instance.CreateEmitter( position, angle, parent, particleController.GetParticles(particleName) );

        return emitter;
    }

    #endregion
}
