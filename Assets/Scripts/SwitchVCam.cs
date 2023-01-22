using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityboost = 10;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera= GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();

        aimAction.canceled += _ => CancelAim();
    }


    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();

        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityboost;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityboost;
    }
}
