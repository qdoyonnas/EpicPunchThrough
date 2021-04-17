using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTechStateChangeOptions: StateChangeStrategyOptions
{
    public override StateChangeStrategy GenerateStrategy()
    {
        return new EndTechStateChange();
    }
}