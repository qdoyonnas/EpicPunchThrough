using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveUpdateOptions : UpdateTechStrategyOptions {
    public Direction direction;
    public float maxSpeed;
    public float acceleration;

    public override void InspectorDraw()
    {
		base.InspectorDraw();

        direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
        maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
        acceleration = EditorGUILayout.FloatField("Acceleration", acceleration);
    }

    public override UpdateTechStrategy GenerateStrategy()
    {
        return new MoveUpdate(inverseStates, validStates, direction, maxSpeed, acceleration);
    }
}
