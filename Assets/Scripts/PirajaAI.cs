using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PirajaAI : MonoBehaviour
{

    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rigidBody;
    GameObject player;
    GameObject duck;
    GameObject beak;
    Collider collider;

    [Header("Movement")]
    [SerializeField] private float moveForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAngle;

    [Header("Wandering")]
    [SerializeField] private float randomMovementFreq;
    [SerializeField] private float randomMovementForce;

    [Header("Chasing")]
    [SerializeField] private float sightRange;

    [Header("Attacking")]
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private int damageTickRate;
    [SerializeField] private float attackInterval;
    private float timeLastAttack;

    [Header("Fleeing")]
    [SerializeField] private float fleeAngle;
    [SerializeField] private float fleeRange;

    [Header("Caught")]
    [SerializeField] private float beakOffset;

    [Header("Flying")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float pirajaHeight;

    [Header("Respawn")]
    [SerializeField] private LayerMask whatIsKitchenDecoration;
    [SerializeField] private int respawnDelay;
    [SerializeField] private int respawnRadius;
    private int collidesInARow;

    private string state;

    // Start is called before the first frame update
    void Start()
    {
        // Statemachine variables
        if (state != "flying")
        {
            state = "wandering";
        }
        collider = GetComponent<Collider>();
        // Component variables
        rigidBody = GetComponent<Rigidbody>();
        duck = GameObject.FindGameObjectsWithTag("Duck")[0];
        
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(rigidBody.velocity.magnitude > 0.1)
        {
            transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
        }
        StateMachine();
    }

    void StateMachine()
    {
        //setting chasing if close to player
        Vector3 parajaToPlayerVec = duck.transform.position - rigidBody.transform.position;
        float distanceToPlayer = parajaToPlayerVec.magnitude;

        //print(distanceToPlayer);

        if (state != "caught" && state != "flying" && state != "deadDuck")
        {
            // can see player
            if (state != "chasing" && distanceToPlayer < sightRange && distanceToPlayer > attackRange)
            {
                state = "chasing";
                //print("New state: " + state);
            }
            // loses sight
            if ((state == "chasing" || state == "fleeing") && distanceToPlayer > sightRange)
            {
                state = "wandering";
                //print("New state: " + state);
            }
            // starts to attack
            if ((state == "chasing" || state == "fleeing") && state != "attacking" && distanceToPlayer < attackRange)
            {
                state = "attacking";
                //print("New state: " + state);
            }
            if (state != "fleeing" && distanceToPlayer < sightRange && DoesPlayerSeeFish())
            {
                state = "fleeing";
                //print("New state: " + state);
            }
        }

        // Update state
        switch (state)
        {
            case "chasing":
                Chase();
                break;
            case "wandering":
                Wander();
                break;
            case "fleeing":
                Flee();
                break;
            case "attacking":
                Attack();
                break;
            case "caught":
                AttachToBeak();
                break;
            case "deadDuck":
                Attack();
                break;
        }
    }

    public void SetCaught(GameObject _beak)
    {
        beak = _beak;
        state = "caught";
        rigidBody.useGravity = false;
        //print("New state: " + state);
    }

    public void SetReleased(Vector3 throwDir, float throwForce)
    {
        state = "flying";
        //print("New state: " + state);
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(throwDir * throwForce, ForceMode.Force);
        rigidBody.useGravity = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (ShouldRespawn())
        {
            Respawn();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if ((state == "flying" || state == "caught"))
        {
            if (collision.gameObject.tag == "Player")
            {

                Physics.IgnoreCollision(duck.GetComponent<Collider>(), collider);
            }
            //print("Collision enter");
            // Check if touch water
            bool grounded = Physics.Raycast(transform.position, Vector3.down, pirajaHeight * 0.5f + 0.2f, whatIsGround);
            if (grounded && state != "caught")
            {
                if (tag == "Point")
                {
                    tag = "Piraja";
                }
                state = "wandering";
                //print("touch ground");
            }

            // Check if touch kitchen decoration

        }
    }

    // Check if located ontop of kitchen and need to respawn
    bool ShouldRespawn()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, pirajaHeight * 0.5f + 0.2f, whatIsKitchenDecoration);
        if (grounded)
        {
            collidesInARow++;
        }
        else
        {
            collidesInARow = 0;
        }

        if (collidesInARow >= respawnDelay)
        {
            return true;
        }
        return false;
    }

    void Respawn()
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);

        transform.position = new Vector3(respawnRadius * x, 7, respawnRadius * z);
    }


    void AttachToBeak()
    {
        transform.rotation = Quaternion.LookRotation(duck.transform.right);
        transform.position = beak.transform.position + duck.transform.forward * 0.5f; //+ player.transform.forward * beakOffset;

        //print("piraja: " + transform.position);
        //print("beak: " + beak.transform.position);
    }

    void Chase()
    {
        Vector3 diffVec = duck.transform.position - rigidBody.transform.position;
        moveDirection = diffVec.normalized;
        SwimInDirection(moveDirection, moveForce);
    }

    void Flee()
    {
        Vector3 diffVec = rigidBody.transform.position - duck.transform.position;
        moveDirection = diffVec.normalized;
        SwimInDirection(moveDirection, moveForce);
    }

    void Attack()
    {
        Chase();
        if (Time.fixedTime - timeLastAttack >= attackInterval)
        {
            duck.GetComponent<DuckController>().DoDamage(attackDamage);
            GetComponent<AudioSource>().Play();
            timeLastAttack = Time.fixedTime;
        }
    }

    void Wander()
    {
        float rand = Random.Range(0.0f, 1.0f);

        moveDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;

        if (rand < randomMovementFreq)
        {
            //print(moveDirection);
            //print("random movement");
            SwimInDirection(moveDirection, randomMovementForce);
        }
    }

    void SwimInDirection(Vector3 direction, float force)
    {
        rigidBody.AddForce(direction * force * 10f, ForceMode.Force);
        if (rigidBody.velocity.magnitude >= maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }
        transform.rotation = Quaternion.LookRotation(direction);
    }

    bool DoesPlayerSeeFish()
    {
        Vector3 PlayerToPirajaVec = rigidBody.transform.position - duck.transform.position;

        float angle = Mathf.Acos(Vector3.Dot(PlayerToPirajaVec.normalized, duck.transform.forward));
        //print("angle: " +angle + " < " + fleeAngle* Mathf.PI / 180);

        return (angle < fleeAngle * Mathf.PI / 180);
    }

    public void SetState(string newState)
    {
        state = newState;
        //print("New state: " + state);
    }

}
