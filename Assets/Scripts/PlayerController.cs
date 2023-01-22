using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    // Adjustable Speed, Jump and gravity
   [SerializeField] private float playerSpeed = 2.0f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityValue = -9.81f;
   [SerializeField] private float rotationSpeed = 10f;

    // References
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

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
        cameraTransform= Camera.main.transform;
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

        // Move in Direction of camera
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;

        // To be sure that Y doesnt get any weird value
        move.y = 0f;

        // Move character
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if ( jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate player to rotation of Camera
        float targetAngle = cameraTransform.eulerAngles.y;
        
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}