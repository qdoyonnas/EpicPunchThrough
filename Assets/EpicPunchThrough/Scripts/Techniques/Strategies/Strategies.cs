using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class TechStrategy
{
    public bool inverseStates = true;
    public string[] validStates = new string[0];

    public TechStrategy(bool inverseStates, string[] states)
    {
        this.inverseStates = inverseStates;
        validStates = states;
    }

    public virtual bool ValidateState(Technique tech)
    {
        bool found = false;
        foreach( string state in validStates ) {
            if( state.ToLower() == tech.state.ToLower() ) {
                found = true;
                break;
            }
        }

        return inverseStates ^ found;
    }
}

public abstract class TriggerTechStrategy: TechStrategy {
    public TriggerTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public abstract bool Trigger(Technique tech, float value );
}
public class NoInterruptTrigger: TriggerTechStrategy {
	public NoInterruptTrigger(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override bool Trigger(Technique tech, float value )
    {
        return !tech.owner.ValidActiveTechnique();
    }
}

public abstract class ActivateTechStrategy: TechStrategy {
	protected ActivateTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public abstract void Activate(Technique tech);
}
public class NoActivate : ActivateTechStrategy
{
    public NoActivate(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override void Activate( Technique tech )
    {
        return;
    }
}

public abstract class TechStateChangeStrategy: TechStrategy {
    public TechStateChangeStrategy(bool inverseStates, string[] states) : base(inverseStates, states) { }
    public abstract void OnStateChange( Technique tech, string previousState, string newState );
}
public class NoTechStateChange: TechStateChangeStrategy {
    public NoTechStateChange(bool inverseStates, string[] states) : base(inverseStates, states) { }
	public override void OnStateChange( Technique tech, string previousState, string newState ) {}
}

public abstract class ActionValidateTechStrategy: TechStrategy {
	protected ActionValidateTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public abstract bool Validate(Technique tech, Agent.Action action, float value);
}
public class NoValidate: ActionValidateTechStrategy {
	public NoValidate(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override bool Validate(Technique tech, Agent.Action action, float value)
    {
        return false;
    }
}
public class AllValidate: ActionValidateTechStrategy {
	public AllValidate(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override bool Validate(Technique tech, Agent.Action action, float value)
    {
        return true;
    }
}

public abstract class StateChangeStrategy: TechStrategy {
	protected StateChangeStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public abstract void OnStateChange( Technique tech, Agent.State previousState, Agent.State newState );
}
public class NoStateChange: StateChangeStrategy {
	public NoStateChange(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override void OnStateChange(Technique tech, Agent.State previousState, Agent.State newState) {}
}


public abstract class UpdateTechStrategy: TechStrategy {
    public UpdateTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public abstract void Update(Technique tech, GameManager.UpdateData data, float value );
    public virtual void BeforeUpdate(Technique tech) { }
    public virtual void AfterUpdate(Technique tech) { }
}
public class NoUpdate: UpdateTechStrategy {
    public NoUpdate(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public override void Update(Technique tech, GameManager.UpdateData data, float value )
    {
        tech.owner.HandlePhysics( data );
        return;
    }
}

public abstract class EventTechStrategy: TechStrategy {
    public EventTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public abstract void OnEvent( Technique tech, AnimationEvent e );
}
public class NoEvent: EventTechStrategy {
    public NoEvent(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override void OnEvent( Technique tech, AnimationEvent e)
	{
		return;
	}
}

public abstract class HitTechStrategy: TechStrategy {
    public HitTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public abstract void OnHit( Technique tech, Vector3 pushVector, double breakForce, Vector3 launchVector );
}
public class DefaultHit : HitTechStrategy {
    public DefaultHit(bool inverseStates, string[] states) : base(inverseStates, states) {}
	public override void OnHit(Technique tech, Vector3 pushVector, double breakForce, Vector3 launchVector)
	{
        if( !ValidateState(tech) ) { return; }

		tech.owner.ProcessHit(pushVector, breakForce, launchVector);
	}
}

public abstract class ExitTechStrategy: TechStrategy {
    public ExitTechStrategy(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public abstract void Exit( Technique tech );
}
public class NoExit : ExitTechStrategy {
    public NoExit(bool inverseStates, string[] states) : base(inverseStates, states) {}
    public override void Exit( Technique tech ) {
        return;
    }
}