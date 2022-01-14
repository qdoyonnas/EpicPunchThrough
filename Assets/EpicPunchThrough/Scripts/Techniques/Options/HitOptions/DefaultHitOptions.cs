using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultHitOptions : HitTechStrategyOptions
{
    public override HitTechStrategy GenerateStrategy()
    {
        return new DefaultHit(inverseStates, validStates);
    }
}