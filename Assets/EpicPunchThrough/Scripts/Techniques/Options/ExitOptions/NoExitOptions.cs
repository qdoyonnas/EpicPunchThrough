using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoExitOptions : ExitTechStrategyOptions
{
    public override ExitTechStrategy GenerateStrategy()
    {
        return new NoExit();
    }
}