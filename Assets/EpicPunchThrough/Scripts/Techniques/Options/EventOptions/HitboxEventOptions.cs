using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HitboxEventOptions : EventTechStrategyOptions
{
	public string eventKey;
    public Direction launchDirection = Direction.Forward;
    public float pushForce = 1;
    public float breakForce = 1;
    public float launchForce = 1;
    public float inertiaCarry = 1;
	public GameObject[] hitboxes = new GameObject[0];
    
    bool show = true;

	public override void InspectorDraw()
	{
		eventKey = EditorGUILayout.TextField("Event Key", eventKey);

        launchDirection = (Direction)EditorGUILayout.EnumPopup("Direction", launchDirection);
        pushForce = EditorGUILayout.FloatField("Push Force", pushForce);
        breakForce = EditorGUILayout.FloatField("Break Force", breakForce);
        launchForce = EditorGUILayout.FloatField("Launch Force", launchForce);
        inertiaCarry = EditorGUILayout.FloatField("Inertia Carry", inertiaCarry);
		
		EditorGUILayout.BeginHorizontal();
        show = EditorGUILayout.Foldout(show ,"Hitboxes");
        int length = EditorGUILayout.IntField(hitboxes.Length);
        EditorGUILayout.EndHorizontal();

        if( show ) {
            EditorGUILayout.BeginVertical();
            for( int i = 0; i < hitboxes.Length; i++ ) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(50));
                hitboxes[i] = (GameObject)EditorGUILayout.ObjectField(hitboxes[i], typeof(GameObject), false);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        if( length != hitboxes.Length ) {
            GameObject[] hits = hitboxes;
            hitboxes = new GameObject[length];
            for(int i = 0; i < length; i++) {
                if( i < hits.Length ) {
                    hitboxes[i] = hits[i];
                }
            }
        }
	}

	public override EventTechStrategy GenerateStrategy()
	{
		return new HitboxEvent(eventKey, launchDirection, pushForce, breakForce, launchForce, inertiaCarry, hitboxes);
	}
}
