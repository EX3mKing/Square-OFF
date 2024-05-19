using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 10f;
    public float dashStrength = 20f;
    public float dashTime = 5f;
    public float dashCooldown = 2f;
    public bool canDash = true;
    public float jumpPower = 7f;
    public int jumpNumber = 2;
    private int _jumpCurrent = 0;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public Slider sensitivitySlider;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;

    // freezes current player moveDirection
    // used for stopping at a jump height to dash
    public bool updateFreeze = false;


    private CharacterController _characterController;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        lookSpeed = PlayerPrefs.GetFloat("Sensitivity", 1f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        #region Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        float curSpeedX =  moveSpeed * input.x ;
        float curSpeedY =  moveSpeed * input.y ;
        float movementDirectionY = _moveDirection.y;
        if (!updateFreeze) _moveDirection = (forward * curSpeedY) + (right * curSpeedX);

        #endregion

        #region Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !updateFreeze && canDash && _moveDirection.magnitude > 0)
        {
            _moveDirection = (forward * curSpeedY + right * curSpeedX) * dashStrength;
            StartCoroutine(DashCooldown());
            StartCoroutine(MovementFreezer(dashTime));
        }
        
        #endregion

        #region Jumping
        
        if (_characterController.isGrounded )
        {
            _jumpCurrent = jumpNumber;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
            _moveDirection.y -= gravity * Time.deltaTime;
        }
        
        if (Input.GetButtonDown("Jump") && _jumpCurrent > 0 && !updateFreeze)
        {
            _jumpCurrent--;
            _moveDirection.y = jumpPower;
        }

        #endregion

        #region Rotation
        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        #endregion

        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    
    // sets the update to freeze than unfreezes after a certain time
    private IEnumerator MovementFreezer (float time)
    {
        updateFreeze = true;
        yield return new WaitForSeconds(time);
        updateFreeze = false;
    }
    
    private IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    private void SetSensitivity(float value)
    {
        lookSpeed = value;
    }
    
}
