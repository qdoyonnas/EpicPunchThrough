using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[Serializable]
public class TechStrategyOptions : ScriptableObject
{
    public virtual void InspectorDraw() {}
}

[Serializable]
public class TriggerTechStrategyOptions : TechStrategyOptions
{
    public virtual TriggerTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ActivateTechStrategyOptions: TechStrategyOptions
{
    public virtual ActivateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ActionValidateTechStrategyOptions: TechStrategyOptions
{
    public virtual ActionValidateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class StateChangeStrategyOptions: TechStrategyOptions
{
    public virtual StateChangeStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class UpdateTechStrategyOptions: TechStrategyOptions
{
    public virtual UpdateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class EventTechStrategyOptions: TechStrategyOptions
{
    public virtual EventTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ExitTechStrategyOptions: TechStrategyOptions
{
    public virtual ExitTechStrategy GenerateStrategy()
    {
        return null;
    }
}
