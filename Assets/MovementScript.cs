using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementScript : MonoBehaviour
{
    PlayerInput input;
    CharacterController Player;
    Vector3 InputDirection;
    Vector2 CurrentMovementInput;
    [SerializeField] float Speed = 3f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        Player = GetComponent<CharacterController>();
        
    }

    private void LateUpdate()
    {
        Movement();
    }

    public void Movement()
    {
            
            InputDirection = new Vector3(CurrentMovementInput.x, 0, CurrentMovementInput.y);

            Player.Move(InputDirection*Time.deltaTime*Speed);
        Debug.Log(InputDirection);
    }

    public void OnMovementInput(InputAction.CallbackContext CTX)
    {
        CurrentMovementInput= CTX.ReadValue<Vector2>(); 
    }
}
