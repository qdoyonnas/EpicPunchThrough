using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetBlackboardActivateOptions : ActivateTechStrategyOptions
{
	private bool show = true;

	public enum DataType {
		STRING,
		INT,
		FLOAT,
		DOUBLE
	}

	[Serializable]
	public class DataEntry {
		public string name;
		public DataType type;
		public string value;
	}
	public DataEntry[] data = new DataEntry[0];

	public override void InspectorDraw()
	{
		base.InspectorDraw();

		EditorGUILayout.BeginHorizontal();
        show = EditorGUILayout.Foldout(show ,"Blackboard Data");
        int length = EditorGUILayout.IntField(data.Length);
        EditorGUILayout.EndHorizontal();

		if( show ) {
			foreach( DataEntry entry in data ) {
				EditorGUILayout.BeginHorizontal();
				entry.name = EditorGUILayout.TextField(entry.name);
				entry.type = (DataType)EditorGUILayout.EnumPopup(entry.type);
				entry.value = EditorGUILayout.TextField(entry.value);
				EditorGUILayout.EndHorizontal();
			}
		}

		if( length != data.Length ) {
            DataEntry[] entries = data;
            data = new DataEntry[length];
            for(int i = 0; i < length; i++) {
                if( i < entries.Length ) {
                    data[i] = entries[i];
                } else {
                    data[i] = new DataEntry();
                }
            }
        }
	}

	public override ActivateTechStrategy GenerateStrategy()
	{
		Dictionary<string, object> generatedData = new Dictionary<string, object>();

		foreach( DataEntry entry in data ) {
			object value = null;
			switch( entry.type ) {
				case DataType.STRING:
					value = entry.value;
					break;
				case DataType.DOUBLE:
					double doubleValue;
					double.TryParse(entry.value, out doubleValue);
					value = doubleValue;
					break;
				case DataType.FLOAT:
					float floatValue;
					float.TryParse(entry.value, out floatValue);
					value = floatValue;
					break;
				case DataType.INT:
					int intValue;
					int.TryParse(entry.value, out intValue);
					value = intValue;
					break;
			}
			generatedData.Add(entry.name, value);
		}

		return new SetBlackboardActivate(inverseStates, validStates, generatedData);
	}
}
