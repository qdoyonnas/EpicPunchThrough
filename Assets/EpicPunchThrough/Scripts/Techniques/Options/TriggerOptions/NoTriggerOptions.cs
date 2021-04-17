using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTriggerOptions : TriggerTechStrategyOptions
{
    public override TriggerTechStrategy GenerateStrategy()
    {
        return new NoTrigger();
    }
}
