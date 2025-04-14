using System;
using System.Collections;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    [Header("Door Settings")] 
    [SerializeField] private Transform doorMesh;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float _doorMoveSpeed = .01f;
    
    private Vector3 originPosition;
    private bool _isOpened;
    private float _doorMoveAlpha;
    private WaitForSeconds _doorTickTime = new WaitForSeconds(.01f);
    
    void Start()
    {
         originPosition = doorMesh.localPosition;
    }

    IEnumerator DoorMoveTick(float alphaChange)
    {
        yield return _doorTickTime;
        _doorMoveAlpha = Mathf.Clamp(_doorMoveAlpha += alphaChange, 0, 1f);
        doorMesh.localPosition = Vector3.Lerp(originPosition, targetPosition.localPosition, _doorMoveAlpha);
        
        if (_doorMoveAlpha == 1f)
        {
            _isOpened = true;
        }
        else if (_doorMoveAlpha == 0f)
        {
            _isOpened = false;
        }
        else
        {
            StartCoroutine(DoorMoveTick(alphaChange));
        }
    }

    public void OpenDoor()
    {
        if (!_isOpened)
        {
            StartCoroutine(DoorMoveTick(_doorMoveSpeed));
        }
    }

    public void CloseDoor()
    {
        if (_isOpened)
        {
            StartCoroutine(DoorMoveTick(-_doorMoveSpeed));
        }
    }
}
