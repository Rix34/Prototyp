using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PrototypRTS _input;

    private Camera PlayerCamera;







    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
