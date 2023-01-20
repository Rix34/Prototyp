using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private Transform cameraTransform;
    [SerializeField] private Vector2 cameraSensitivity = new Vector2(50,50); //lookspeed

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [SerializeField] private float TopClamp = 70.0f;
    [SerializeField] private float BottomClamp = -30.0f;

    [SerializeField] private GameObject CinemachineCameraTarget;
    private float CameraAngleOverride = 0.0f;
    private const float _threshold = 0.00001f;
    [SerializeField] private bool LockCameraPosition = false;
    private Vector2 look;
    public bool cameraHold;
    private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";
    void Start()
    {
        //_input = new InputActions_Walker();
        //_input.Player.Enable();
        cameraTransform = Camera.main.transform;
        _playerInput = GetComponent<PlayerInput>();

        
    }
    void Update()
    {
        //Debug.Log(_playerInput.currentControlScheme);
    }
    void LateUpdate()
    {
        CameraRotation();
    }
    public void GetInput(InputAction.CallbackContext ctx) 
    {
        look = ctx.ReadValue<Vector2>();
    }
    public void CameraRotation()
    {
        if (cameraHold)
            return;
        if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _cinemachineTargetYaw +=  cameraSensitivity.x* look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += -cameraSensitivity.y * look.y * deltaTimeMultiplier;
        }

        /*// if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _cinemachineTargetYaw += 3 * _input.look.x * deltaTimeMultiplier * _cameraSensitivity;
            _cinemachineTargetPitch += 3 * _input.look.y * deltaTimeMultiplier * _cameraSensitivity;
        }*/

        // 
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


}
