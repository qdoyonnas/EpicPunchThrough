using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;

    [Tooltip("Leave blank for auto-find")]
    protected CameraBase _cameraBase;
    public CameraBase cameraBase {
        get {
            FindCamera();
            return _cameraBase;
        }
        set {
            _cameraBase = value;
        }
    }

    private void Start()
    {
        FindCamera();

        GameManager.Instance.updated += DoFixedUpdate;
    }
    private void OnDisable()
    {
        if( GameManager.Instance != null ) {
            GameManager.Instance.updated -= DoFixedUpdate;
        }
    }
    private void OnEnable()
    {
        FindCamera();

        GameManager.Instance.updated += DoFixedUpdate;
    }

    void FindCamera()
    {
        if( _cameraBase != null ) { return; }

        _cameraBase = GetComponent<CameraBase>();
        if( _cameraBase == null ) {
            _cameraBase = GameManager.Instance.activeCamera;
            if( _cameraBase == null ) {
                Debug.LogError("Spectator Camera could not find CameraBase");
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void DoFixedUpdate(GameManager.UpdateData data)
    {
        // No action
    }
}