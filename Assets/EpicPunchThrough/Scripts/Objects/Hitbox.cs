using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public bool isStatic = false;
    public float lifeTime = 1;
    float lifeTimestamp = 0;

    public void OnUpdate( GameManager.UpdateData data )
    {
        if( Time.time >= lifeTimestamp ) {
            Destroy(gameObject);
        }
    }

	private void OnDestroy()
	{
		GameManager.Instance.fixedUpdated -= OnUpdate;
	}

	public void Init(double attackSpeed)
    {
        lifeTimestamp = Time.time + (lifeTime * (float)attackSpeed);
        GameManager.Instance.fixedUpdated += OnUpdate;
    }
}
