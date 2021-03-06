using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsBody), typeof(SpriteRenderer))]
public class Prop : MonoBehaviour
{
    public bool isPassable = true;

    protected PhysicsBody _physicsBody;
    public PhysicsBody physicsBody {
         get {
            return _physicsBody;
        }
    }
    protected Collider[] colliders;

    protected void Start()
    {
        colliders = GetComponentsInChildren<Collider>();

        _physicsBody = GetComponent<PhysicsBody>();

        PropsManager.Instance.RegisterProp(this);
    }

	protected void OnDestroy()
	{
		PropsManager.Instance.UnregisterProp(this);
	}

	public void DoUpdate( GameManager.UpdateData data )
    {

    }
}
