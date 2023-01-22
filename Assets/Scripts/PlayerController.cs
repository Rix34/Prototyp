using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    // Adjustable Speed, Jump and gravity
   [SerializeField] private float playerSpeed = 2.0f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityValue = -9.81f;

    // References
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // InputSystem Actions
    private InputAction moveAction;
    private InputAction jumpAction;
    //private InputAction lookAction;
    //private InputAction aimAction;
    //private InputAction shootAction;


    private void Start()
    {

        //Get Components to controll them
        controller = GetComponent<CharacterController>();
        playerInput= GetComponent<PlayerInput>();

        // Match Input with simplified name
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        // Read Value from Input to change direction of move
        Vector2 input = moveAction.ReadValue<Vector2>();
        // Change Direction to match input
        Vector3 move = new Vector3(input.x, 0 , input.y);
        // Move character
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if ( jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}