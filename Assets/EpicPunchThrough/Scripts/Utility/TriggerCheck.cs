using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCheck : MonoBehaviour
{
    public bool externalTriggerCount = false;
    [Serializable]
    public class TriggerAction : UnityEvent<bool, Collider> {}
    public TriggerAction onTrigger = new TriggerAction();

    Collider _collider;
    public new Collider collider {
        get {
            return _collider;
        }
    }

    public bool doDetect {
        get {
            return collider.enabled;
        }
        set {
            collider.enabled = value;
            if( !value ) {
                onTrigger.Invoke( false, null );
            }
        }
    }

    protected int _triggerCount = 0;
    public int triggerCount {
        get {
            return _triggerCount;
        }
        set {
            if( externalTriggerCount ) {
                _triggerCount = value;
            }
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter( Collider other )
    {
        if( !externalTriggerCount ) { _triggerCount++; }
        onTrigger.Invoke( true, other );
    }
    private void OnTriggerExit( Collider other )
    {
        if( !externalTriggerCount ) { _triggerCount--; }
        onTrigger.Invoke( false, other );
    }
}
