using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUpdateOptions: UpdateTechStrategyOptions
{
    public override void InspectorDraw() {}

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new NoUpdate(inverseStates, validStates);
    }
}