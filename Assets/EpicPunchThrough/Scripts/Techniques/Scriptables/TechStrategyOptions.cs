using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[Serializable]
public class TechStrategyOptions : ScriptableObject
{
    public int order = 1;

    public bool inverseStates = true;
    public string[] validStates = new string[0];

    private bool showStates = true;

    public virtual void InspectorDraw()
    {
        inverseStates = EditorGUILayout.Toggle("Inverse States", inverseStates);

        EditorGUILayout.BeginHorizontal();
        showStates = EditorGUILayout.Foldout(showStates, "Technique States");
        int length = EditorGUILayout.IntField(validStates.Length);
        EditorGUILayout.EndHorizontal();

        if( showStates ) {
            EditorGUILayout.BeginVertical();
            string info = "";
            if( validStates.Length == 0 ) {
                info = inverseStates ? "Valid in ALL states" : "Valid in NO states";
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(info);
                EditorGUI.indentLevel--;
            } else {
                info = inverseStates ? "Not Valid in following states:" : "Valid in following states:";
                EditorGUILayout.LabelField(info);
            }

            for( int i = 0; i < validStates.Length; i++ ) {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                validStates[i] = EditorGUILayout.TextField("State", validStates[i]);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        if( length != validStates.Length ) {
            string[] states = validStates;
            validStates = new string[length];
            for(int i = 0; i < length; i++) {
                if( i < states.Length ) {
                    validStates[i] = states[i];
                } else {
                    validStates[i] = String.Empty;
                }
            }
        }

        EditorGUILayout.Space(20);
    }
}

[Serializable]
public class TriggerTechStrategyOptions : TechStrategyOptions
{
    public virtual TriggerTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ActivateTechStrategyOptions: TechStrategyOptions
{
    public virtual ActivateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ActionValidateTechStrategyOptions: TechStrategyOptions
{
    public virtual ActionValidateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class TechStateChangeStrategyOptions: TechStrategyOptions
{
    public virtual TechStateChangeStrategy GenerateStrategy()
    {
        return null;
    }
}


[Serializable]
public class StateChangeStrategyOptions: TechStrategyOptions
{
    public virtual StateChangeStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class UpdateTechStrategyOptions: TechStrategyOptions
{
    public virtual UpdateTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class EventTechStrategyOptions: TechStrategyOptions
{
    public virtual EventTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class HitTechStrategyOptions: TechStrategyOptions
{
    public virtual HitTechStrategy GenerateStrategy()
    {
        return null;
    }
}

[Serializable]
public class ExitTechStrategyOptions: TechStrategyOptions
{
    public virtual ExitTechStrategy GenerateStrategy()
    {
        return null;
    }
}
