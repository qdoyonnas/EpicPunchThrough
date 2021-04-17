using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoValidateOptions: ActionValidateTechStrategyOptions
{
    public override ActionValidateTechStrategy GenerateStrategy()
    {
        return new NoValidate();
    }
}