using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllValidateOptions: ActionValidateTechStrategyOptions
{
    public override ActionValidateTechStrategy GenerateStrategy()
    {
        return new AllValidate(inverseStates, validStates);
    }
}
