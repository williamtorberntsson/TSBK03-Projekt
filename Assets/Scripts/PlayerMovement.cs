using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    public bool controlsEnabled;

    Vector3 moveDirection;
    Rigidbody rigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        controlsEnabled = true;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(controlsEnabled){

            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight* 0.5f + 0.2f, whatIsGround);
            MyInput();

            // handle drag
            if (grounded)
                rigidBody.drag = groundDrag;
            else
                rigidBody.drag = 0;
        }
    }

    // Updates on every physics update
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }


    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
