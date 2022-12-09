using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraInputController cameraInputs;
    private InputAction movementCamera;
    private Transform cameraTransform;

    [Header("WorldBorder")]
    [SerializeField]
    public Vector2 worldBorder;

    [Header("Horizontal motion")]
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    [Header("Vertical motion - zoom")]
    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;

    [Header("Rotation")]
    [SerializeField]
    private float maxRotationSpeed = 1f;

    [Header("Screen edge motion")]
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private bool useScreenEdge = true;

    private Vector3 targetlookPosition;

    private float zoomHeight;

    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;
   

    Vector3 startDragPosition;

    private void Awake()
    {
        cameraInputs = new CameraInputController();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
     

      
    }
    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);
        lastPosition = this.transform.position;
        movementCamera = cameraInputs.CameraActions.MovementCamera;
        cameraInputs.CameraActions.RotateCamera.performed += RotateCamera;
        cameraInputs.CameraActions.ZoomCamera.performed += ZoomCamera;

        cameraInputs.CameraActions.Enable();
        
    }

    private void OnDisable()
    {
        cameraInputs.CameraActions.RotateCamera.performed -= RotateCamera;
        cameraInputs.CameraActions.ZoomCamera.performed -= ZoomCamera;
        cameraInputs.Disable();
    }

    

    private void Update()
    {
        GetKeyboardMovement();
        if (useScreenEdge)
        {
            CheckMouseAtScreen();
        }
        DragCamera();
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;     
    }
    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movementCamera.ReadValue<Vector2>().x * GetCameraRight() + movementCamera.ReadValue<Vector2>().y * GetCameraForward();
        
        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetlookPosition += inputValue;
       
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
       

        if (targetlookPosition.sqrMagnitude > 0.1f)
        {
            
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetlookPosition * speed * Time.deltaTime;
           // targetlookPosition.x = Mathf.Clamp(targetlookPosition.x, -worldBorder.x, worldBorder.x);
            //targetlookPosition.z = Mathf.Clamp(targetlookPosition.z, -worldBorder.y, worldBorder.y);
           // transform.position = targetlookPosition;

        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
          //  targetlookPosition.x = Mathf.Clamp(targetlookPosition.x, -worldBorder.x, worldBorder.x);
            //targetlookPosition.z = Mathf.Clamp(targetlookPosition.z, -worldBorder.y, worldBorder.y);
           // transform.position = targetlookPosition;
        }
       

       targetlookPosition = Vector3.zero;      
    }

    private void CheckMouseAtScreen() 
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetlookPosition += moveDirection;
       
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

            float value = inputValue.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
        

    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (MathF.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if (zoomHeight > maxHeight)
                zoomHeight = maxHeight;
        }
    }
    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);


        // Making arc flight movement illusion
       // zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }
    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
                startDragPosition = ray.GetPoint(distance);
            else
                targetlookPosition += startDragPosition - ray.GetPoint(distance);
        }
    }


}
