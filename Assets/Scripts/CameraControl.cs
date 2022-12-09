using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private CameraInputController cameraController;
    private InputAction cameraMove;
    private Transform cameraTransform;

   
    

    [SerializeField]
    private float camspeed = 20f;
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private float scrollSpeed = 200f;

    public Vector2 worldBorderCam;
    private Vector3 currPosition;
    private float zoomHeight;
    private float stepSize = 2f;
    private float minHeight = 10f;
    private float maxHeight = 60f;

    private void Awake()
    {
        cameraController = new CameraInputController();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        cameraTransform.LookAt(this.transform);
        cameraMove = cameraController.CameraActions.MovementCamera;
        cameraController.CameraActions.ZoomCamera.performed += ZoomCamera;
        cameraController.CameraActions.Enable();
    }

    private void OnDisable()
    {
        cameraController.CameraActions.ZoomCamera.performed -= ZoomCamera;
        cameraController.Disable();
    }

   
    void Update()
    {
       
        KeyBoardMovement();
        CheckMouseAtScreen();
        BasePos();
       
       
    }
    private void BasePos()
    {
        // transform.position += targetPosition * camspeed * Time.deltaTime;
        //targetPosition = Vector3.zero;
        
       

    }


    private void KeyBoardMovement()
    {
        Vector3 inputValue = cameraMove.ReadValue<Vector2>().x * GetCameraRight() + cameraMove.ReadValue<Vector2>().y * GetCameraForward();
        currPosition += inputValue * camspeed * Time.deltaTime;

        currPosition.x = Mathf.Clamp(currPosition.x, -worldBorderCam.x, worldBorderCam.x);
        currPosition.z = Mathf.Clamp(currPosition.z, -worldBorderCam.y, worldBorderCam.y);
        transform.position = currPosition;
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

        currPosition += moveDirection * camspeed * Time.deltaTime;
        currPosition.x = Mathf.Clamp(currPosition.x, -worldBorderCam.x, worldBorderCam.x);
        currPosition.z = Mathf.Clamp(currPosition.z, -worldBorderCam.y, worldBorderCam.y);
        transform.position = currPosition;
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        //float value = -inputValue.ReadValue<Vector2>().y;

        //zoomHeight = cameraTransform.localPosition.y + value * scrollSpeed;
        //cameraTransform.LookAt(this.transform);

        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (MathF.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if (zoomHeight > maxHeight)
                zoomHeight = maxHeight;

            //cameraTransform.localPosition.y = zoomHeight;
        }

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
}
