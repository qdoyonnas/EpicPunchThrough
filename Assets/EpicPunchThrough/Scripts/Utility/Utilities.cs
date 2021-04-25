using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilities
{
    public static bool DoesSceneExist( string sceneName )
    {
        List<string> sceneNames = new List<string>();
        for( int i = 0; i < SceneManager.sceneCountInBuildSettings; i++ ) {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            int lastDot = scenePath.LastIndexOf(".");
            sceneNames.Add( scenePath.Substring(lastSlash + 1, lastDot - lastSlash - 1) );
        }

        return sceneNames.Contains(sceneName);
    }

    public static Vector3 GetDirectionVector( Agent agent, Direction direction )
    {
        Vector3 vector = Vector3.zero;
        switch( direction ) {
            case Direction.Aim:
                vector = agent.aimDirection;
                break;
            case Direction.Back:
                vector = agent.transform.right * (agent.isFacingRight ? -1 : 1);
                break;
            case Direction.Forward:
                vector = agent.transform.right * (agent.isFacingRight ? 1 : -1);
                break;
            case Direction.Down:
                vector = agent.transform.up * -1;
                break;
            case Direction.Up:
                vector = agent.transform.up;
                break;
        }

        return vector;
    }

    public static float VFToForce(double vf, float minForce = -1, float maxForce = -1)
    {
        float force = (float)(vf * 0.05);

        if( minForce != -1 && force < minForce ) {
			force = minForce;
		}
		if( maxForce != -1 && force > maxForce ) {
			force = maxForce;
		}

        return force;
    }
}
