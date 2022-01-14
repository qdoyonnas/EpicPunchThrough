using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoExitOptions : ExitTechStrategyOptions
{
    public override void InspectorDraw() {}

    public override ExitTechStrategy GenerateStrategy()
    {
        return new NoExit(inverseStates, validStates);
    }
}