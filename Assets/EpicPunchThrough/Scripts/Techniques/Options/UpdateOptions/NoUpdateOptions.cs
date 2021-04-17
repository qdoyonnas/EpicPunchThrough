using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUpdateOptions: UpdateTechStrategyOptions
{
    public override UpdateTechStrategy GenerateStrategy()
    {
        return new NoUpdate();
    }
}