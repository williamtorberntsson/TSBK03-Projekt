using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    [Header("KeyBinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

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
        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        if(controlsEnabled){

            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight* 0.5f + 0.2f, whatIsGround);
            MyInput();

            //Debug.Log(grounded);
        // if(Input.GetKey(jumpKey))
            //    Debug.Log(jumpKey);

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

        // Can jump?
       /* if(Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); // Reset readyToJump after a certain time
        }*/
    }


    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else // in air
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void Jump() {
        // Set y velocity to 0
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse); // only apply force once
    }

    private void ResetJump() {
        readyToJump = true;
    }
}
