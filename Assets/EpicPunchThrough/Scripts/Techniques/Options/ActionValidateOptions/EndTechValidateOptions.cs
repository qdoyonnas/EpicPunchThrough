using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndTechValidateOptions: ActionValidateTechStrategyOptions
{
    public bool skipExit = false;
    public ActionState[] actionStates = new ActionState[0];

    private bool show = true;

    public override void InspectorDraw()
    {
        skipExit = EditorGUILayout.Toggle("Skip Exit", skipExit);

        EditorGUILayout.BeginHorizontal();
        show = EditorGUILayout.Foldout(show ,"Actions");
        int length = EditorGUILayout.IntField(actionStates.Length);
        EditorGUILayout.EndHorizontal();

        if( show ) {
            EditorGUILayout.BeginVertical();
            for( int i = 0; i < actionStates.Length; i++ ) {
                EditorGUILayout.BeginHorizontal();
                actionStates[i].action = (Agent.Action)EditorGUILayout.EnumPopup(actionStates[i].action);
                actionStates[i].state = EditorGUILayout.Toggle(actionStates[i].state);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        if( length != actionStates.Length ) {
            ActionState[] actions = actionStates;
            actionStates = new ActionState[length];
            for(int i = 0; i < length; i++) {
                if( i < actions.Length ) {
                    actionStates[i] = actions[i];
                } else {
                    actionStates[i] = new ActionState();
                }
            }
        }
    }

    public override ActionValidateTechStrategy GenerateStrategy()
    {
        return new EndTechValidate(skipExit, actionStates);
    }
}