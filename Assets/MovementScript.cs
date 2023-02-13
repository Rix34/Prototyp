using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    #region BasePlayerStats
    float playerSpeed = 3f;
    float sprintSpeed = 6f;
    #endregion
    #region TemporaryStats

    #endregion
    PlayerController _controller;
    private void Awake() {
        _controller = GetComponent<PlayerController>();
    }
    private void Move(Vector2 input) {
        //Vector2 movementAmount = _controller._input.GetMove;

        float targetSpeed = _controller._input.IsSprinting? playerSpeed : sprintSpeed;
        if(_controller._input.GetMove == Vector2.zero)targetSpeed = 0f;
        Vector3 horizontalVelocity = new Vector3(input.x, 0f, input.y).normalized;


        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
        
    }
    private void LateUpdate() {
        Move(_controller._input.GetMove);
    }
}
