using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoActivateOptions : ActivateTechStrategyOptions
{
    public override ActivateTechStrategy GenerateStrategy()
    {
        return new NoActivate();
    }
}
