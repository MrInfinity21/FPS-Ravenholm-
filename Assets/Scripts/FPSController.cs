using System;
using UnityEngine;

// We use requirecomponent to enforce that this component will be on this gameobject.
// The component will be added automatically to enforce this.
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    private CharacterController _controller;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _gravity = -9.5f;
    private Vector3 _velocity;
    private Vector3 _moveVector;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;
    
    [Header("Rotation")]
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private float _mouseSensitivity = 1f;
    [SerializeField] private float _xCameraBounds = 80f;
    [SerializeField] private Camera _camera;
    private Vector2 _currentMouseDelta;
    private Vector2 _currentMouseVelocity;
    private float _xRotation;


    [Header("Interaction")] 
    [SerializeField] private float _pickupRange = 3f;
    [SerializeField] private Transform _pickupHoldPosition;
    private GameObject _currentPickupObject;
    private bool _isHolidngObject = false;


    [Header("Platform Interaction")] 
    [SerializeField] private GameObject platform;
    [SerializeField] private DoorController doorController;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    void Start()
    {
        // We can change the cursor states using these kind of values.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Movement();
        Rotation();
        HandleInteraction();
        CheckForCubePlacement();
    }

    void Movement()
    {
        // To check the Ground detection is handled by the CharacterController itself.
        _isGrounded = _controller.isGrounded;

        Debug.DrawRay(_groundCheck.position, Vector3.down * _groundDistance, Color.green);
        
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = (float)Math.Sqrt(2f * _gravity * _jumpHeight);
        }
        
        
        Vector3 input = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        input.Normalize();
        Vector3 targetVelocity = input * _movementSpeed;
        
        _velocity.x = Mathf.Lerp(_velocity.x, targetVelocity.x, _acceleration * Time.deltaTime);
        _velocity.z = Mathf.Lerp(_velocity.z, targetVelocity.z, _acceleration * Time.deltaTime);

        if (_controller.isGrounded)
        {
            _velocity.y = -1f;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }
        
        _controller.Move(_velocity * Time.deltaTime);

    }

    void Rotation()
    {
        // Getting the current direction the mouse is traveling in
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        
        // Getting the intended destination from the mouse for this frame
        Vector2 targetDelta = new Vector2(mouseX, mouseY);
        
        // Using the previous frames target delta, move the previous position towards the target.
        // We use smooth damp to smoothly move in this direction, from current to target mouse delta.
        _currentMouseDelta = Vector2.SmoothDamp(
            _currentMouseDelta, 
            targetDelta, 
            ref _currentMouseVelocity, 
            _smoothTime
            );
        
        // Based on the change in our mouse delta Y we can rotate our camera up and down based
        // on this value
        _xRotation -= _currentMouseDelta.y;
        _xRotation = Mathf.Clamp(_xRotation, -_xCameraBounds, _xCameraBounds);
        // Rotating the camera up and down based on the current mouse delta target on the Y
        // The camera will rotate on it's X axis for up and down.
        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        
        // Rotating the character left and right based on the target mouse delta
        transform.Rotate(Vector3.up, _currentMouseDelta.x);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isHolidngObject)
            {
                TryPickupObject();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (_isHolidngObject)
            {
                DropObject();
            }
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _pickupRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == ("Pickup"))
            {
                _currentPickupObject = hit.collider.gameObject;
                PickupObject();
            }    
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }
    }

    void PickupObject()
    {
        _isHolidngObject = true;
        _currentPickupObject.GetComponent<Rigidbody>().isKinematic = true;
        _currentPickupObject.transform.SetParent(_pickupHoldPosition);
        _currentPickupObject.transform.localPosition = Vector3.zero;
    }

    void DropObject()
    {
        _isHolidngObject = false;
        _currentPickupObject.GetComponent<Rigidbody>().isKinematic = false;
        _currentPickupObject.transform.SetParent(null);
        _currentPickupObject = null;
    }

    void CheckForCubePlacement()
    {
        if (_currentPickupObject != null && !_isHolidngObject)
        {
            if (platform.GetComponent<Collider>().bounds.Contains(_currentPickupObject.transform.position))
            {
                doorController.OpenDoor();
            }
            else
            {
                doorController.CloseDoor();
            }
        }
        
    }
}