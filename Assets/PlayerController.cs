using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public InputGatherer _input;

    [HideInInspector]
    public MovementScript _movement;

    [HideInInspector]
    public CombatScript _combat;
    #region BasePlayerStats
    float playerSpeed = 3f;
    float sprintSpeed = 6f;
    #endregion
    #region TemporaryStats

    #endregion
    private void Awake()
    {
        _input = GetComponent<InputGatherer>();
        _movement = GetComponent<MovementScript>();
        _combat = GetComponent<CombatScript>();
    }

    private void LateUpdate() { }
}
