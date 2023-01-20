using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    PlayerInput input;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();

    }

    void Movement(InputAction.CallbackContext CTX)
    {
        if(CTX.started)
        {
            Vector2 ReadDirection = CTX.ReadValue<Vector2>();


        }
    }



}
