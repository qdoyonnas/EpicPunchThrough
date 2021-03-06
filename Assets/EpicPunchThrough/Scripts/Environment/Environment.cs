using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldBounds
{
	public bool leftBound;
	public bool rightBound;
	public bool topBound;
	public bool bottombound;
	public float minX;
	public float maxX;
	public float width
	{
		get { return maxX - minX; }
	}
	public float minY;
	public float maxY;
	public float height
	{
		get { return maxY - minY; }
	}
}

[CreateAssetMenu(menuName = "Project/Environment")]
public class Environment : ScriptableObject
{
    public string sceneName;
    public Vector3 gravity;
    public WorldBounds agentBounds;

    public Vector3 groundFriction;
    public Vector3 airFriction;
    public Vector3 wallFriction;
}
