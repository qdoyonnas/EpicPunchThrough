using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInterruptTriggerOptions : TriggerTechStrategyOptions
{
    public override void InspectorDraw() {}

    public override TriggerTechStrategy GenerateStrategy()
    {
        return new NoInterruptTrigger(inverseStates, validStates);
    }
}
