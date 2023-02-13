using UnityEngine;
using UnityEngine.InputSystem;

public class InputGatherer : MonoBehaviour
{
    Vector2 MoveInput;
    Vector2 LookInput;
    bool isShooting = false;
    bool isSprinting = false;
    #region Properties
    public Vector2 GetMove
    {
        get { return MoveInput; }
    }
    public Vector2 GetLook
    {
        get { return LookInput; }
    }

    public bool IsSprinting { get => isSprinting; set => isSprinting = value; }
    public bool IsShooting { get => isShooting; set => isShooting = value; }
    #endregion
    public static PrototypRTS _input;
    PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _input = new PrototypRTS();
    }

    private void OnEnable()
    {
        _input.Player.Enable();

        _input.Player.Move.performed += Move;
        _input.Player.Move.canceled += Move;

        _input.Player.Look.performed += Look;
        _input.Player.Look.canceled += Look;

        _input.Player.Fire.performed += StartShooting;
        _input.Player.Fire.canceled += StopShooting;

        _input.Player.Sprint.performed += StartSprinting;
        _input.Player.Sprint.canceled += StopSprinting;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= Move;
        _input.Player.Move.canceled -= Move;

        _input.Player.Look.performed -= Look;
        _input.Player.Look.canceled -= Look;

        _input.Player.Fire.performed -= StartShooting;
        _input.Player.Fire.canceled -= StopShooting;

        _input.Player.Sprint.performed -= StartSprinting;
        _input.Player.Sprint.canceled -= StopSprinting;

        _input.Player.Disable();
    }
    #region Movement
    private void Move(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        
    }
    #endregion
    #region Look
    private void Look(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
        
    }
    #endregion
    #region Shooting
    private void StartShooting(InputAction.CallbackContext ctx)
    {
        isShooting = true;
    }
    private void StopShooting(InputAction.CallbackContext ctx)
    {
        isShooting = false;
    }
    #endregion
    #region Sprinting
    private void StartSprinting(InputAction.CallbackContext ctx)
    {
        isSprinting = true;
    }
    private void StopSprinting(InputAction.CallbackContext ctx)
    {
        isSprinting = false;
    }
    #endregion
}
