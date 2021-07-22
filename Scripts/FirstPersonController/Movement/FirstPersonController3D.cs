using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController3D : MonoBehaviour, IMoveable
{
    //Settings Fields=======================================================================================================================================================

    //Camera Controls-------------------------------------------------------------------------------------------------------------------------------------------------------
    [HorizontalLine(color: EColor.Red)]
    [Header("Camera Settings")]

    //Inspector Fields
    [SerializeField, Tooltip("this setting controls how fast will the camera rotate in response to mouse movement")]
    private float _mouseSensitivity = 3.5f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("The amount of smoothing that is applyed when player moves the mouse")]
    private float _mouseShoothTime = 0.03f;
    [SerializeField, MinMaxSlider(-90.0f, 90.0f), Tooltip("X represents min value and Y represents max value that the camera can rotate on Y axis")]
    private Vector2 _cameraClampYAxis = new Vector2(-90, 90);
    [SerializeField]
    private bool _lockCursor = true;

    //Other camera look Fields
    float _cameraPitch = 0f;
    Vector2 _currentMouseDelta = Vector2.zero;
    Vector2 _currentMouseDeltaVelocity = Vector2.zero;

    //Character Movement Controls-------------------------------------------------------------------------------------------------------------------------------------------
    [HorizontalLine(color: EColor.Red)]
    [Header("Movement Settings")]

    //Inspector Fields
    [SerializeField, Tooltip("value that defines movement speed")]
    private float _movementSpeed = 6f;
    [SerializeField, Tooltip("jump force")]
    private float _jumpForce = 200f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("The amount of smoothing that is applyed when player starts or stops moving")]
    private float _movementShoothTime = 0.3f;
    [SerializeField, Tooltip("Determines how much gravity is applied to the character")]
    private float _gravityForce = -13f;
    [SerializeField, Tooltip("GroundCheck size")]
    private float _groundCheckSize = 1f;
    [SerializeField]
    private LayerMask _groundCheckLayer;
    //Other movement Fields
    Vector2 _currentDirection = Vector2.zero;
    Vector2 _currentDirectionVelocity = Vector2.zero;

    //References============================================================================================================================================================
    
    //Inspector Fields
    [Header("References")]
    [SerializeField, Required]
    private Transform _bodyTransform = default;
    [SerializeField, Required]
    private Transform _headTransform = default;
    [SerializeField, Required]
    private CharacterController _characterController = default;
    [SerializeField]
    private ImpactManager _impactManager;

    //other global fields
    private float _movementSpeedMultiplyer = 1f;
    private bool _isGrounded;
    Vector3 _gravityVelocity = Vector3.zero;


    //Initialization Methods ===============================================================================================================================================

    //Unity Awake-----------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
    }

    //Unity Start-----------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Update Methods =======================================================================================================================================================
    
    //Unity Update
    private void Update()
    {
        //UpdateGravity();
        Vector3 velocity = transform.up * _gravityForce;
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        _isGrounded = false;
        Collider[] collider = Physics.OverlapSphere(GetOriginPoint(), _groundCheckSize);
        foreach (var item in collider)
        {
            if ((_groundCheckLayer & 1 << item.gameObject.layer) == 1 << item.gameObject.layer)
                _isGrounded = true;
                    //TODO Make it beautiful
        }
    }


    //Public methods========================================================================================================================================================

    //Data requesters----------------------------------------------------------------------------------------------------------------------------------------

    public Transform GetCameraHolder() => _headTransform;

    public Vector3 GetOriginPoint() => _characterController.transform.position;

    public Vector2 GetRotation()
    {
        Vector2 data = new Vector2(_characterController.transform.eulerAngles.y, _headTransform.eulerAngles.x);
        return data;
    }

    //Character Camera Control Methods--------------------------------------------------------------------------------------------------------------------------------------

    //Changes player look direction based on 0 to 1 input on both axes
    public void ChangeLookOnInput(Vector2 lookDirectionDelta)
    {
        //smoothing mouse
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, lookDirectionDelta, ref _currentMouseDeltaVelocity, _mouseShoothTime);

        //calculating y axis rotation
        _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, _cameraClampYAxis.x, _cameraClampYAxis.y);
        _headTransform.localEulerAngles = Vector3.right * _cameraPitch;

        //calculating x axis rotation
        _bodyTransform.Rotate(Vector3.up * _currentMouseDelta.x * _mouseSensitivity);
    }

    //Character Movement Methods--------------------------------------------------------------------------------------------------------------------------------------------

    //Moves player in relation to it's current forward direction, based on 0 to 1 input on both axes
    public void MovePlayerRelativeOnInput(Vector2 movementDirection)
    {
        movementDirection.Normalize();

        //applying smooting
        _currentDirection = Vector2.SmoothDamp(_currentDirection, movementDirection, ref _currentDirectionVelocity, _movementShoothTime);


        float movementSpeed = _movementSpeed * _movementSpeedMultiplyer;
        Vector3 velocity = (transform.forward * _currentDirection.y + transform.right * _currentDirection.x) * movementSpeed;

        //applying velocity
        _characterController.Move(velocity * Time.deltaTime);
    }

    //Moves player in relation to global forward direction, based on 0 to 1 input on both axes
    public void MovePlayerAbsoluteOnInput(Vector2 movementDirection)
    {
        movementDirection.Normalize();

        //applying smooting
        _currentDirection = Vector2.SmoothDamp(_currentDirection, movementDirection, ref _currentDirectionVelocity, _movementShoothTime);


        float movementSpeed = _movementSpeed * _movementSpeedMultiplyer;
        Vector3 velocity = new Vector3(_currentDirection.x, 0f, _currentDirection.y) * movementSpeed;

        //applying velocity
        _characterController.Move(velocity * Time.deltaTime);
    }

    //Character Force Methods-----------------------------------------------------------------------------------------------------------------------------------------------
    public void Jump()
    {
        if(_isGrounded)
            AddImpactForce(Vector3.up, _jumpForce);
    }

    public void AddImpactForce(Vector3 direction, float force)
    {
        if (_impactManager != null)
            _impactManager.AddImpact(direction, force);
        else
            Debug.LogWarning($"Impact Manager not set on {this}");
    }

    //IMoveable Interface implementation ===================================================================================================================================
    
    //Buff methods----------------------------------------------------------------------------------------------------------------------------------------------------------

    //Changes player speed multiplyer
    public void ChangeMovementSpeedMultiplyer(float newValue)
    {
        _movementSpeedMultiplyer = newValue;
    }

    //Movement manipulation logic-------------------------------------------------------------------------------------------------------------------------------------------

    public void Immobilize()
    {
        throw new System.NotImplementedException();
    }

    public void Push(Vector3 direction, float force)
    {
        AddImpactForce(direction, force);
    }

    public void SetPosition(Vector2 newPos)
    {
        SetPosition(new Vector3(newPos.x, _bodyTransform.position.y, newPos.y));
    }
    public void SetPosition(Vector3 newPos)
    {
        _characterController.enabled = false;
        _bodyTransform.position = new Vector3(newPos.x, newPos.y, newPos.z);
        _characterController.enabled = true;
    }

    //Temporary
    public bool HasFallenOffPlatform()
    {
        return _characterController.transform.position.y <= -0.5f ? true : false;
    }
}
