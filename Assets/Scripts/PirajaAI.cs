using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirajaAI : MonoBehaviour
{

    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rigidBody;
    GameObject player;
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
    private int damageDelay;




    private string state;

    // Start is called before the first frame update
    void Start()
    {
        // Statemachine variables
        state = "wandering";

        // Component variables
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        StateMachine();
        

    }

    void StateMachine()
    {
        //setting chasing if close to player
        Vector3 parajaToPlayerVec = player.transform.position - rigidBody.transform.position;
        float distanceToPlayer = parajaToPlayerVec.magnitude;
        //print(distanceToPlayer);
        if (state != "chasing" && distanceToPlayer < sightRange && distanceToPlayer > attackRange)
        {
            state = "chasing";
            print("New state: " + state);
        }
        //loses sight
        if(state == "chasing" && distanceToPlayer > sightRange){
            state = "wandering";
            print("New state: " + state);
        }
        
        if(state == "chasing" && state != "attacking" && distanceToPlayer < attackRange){
            state = "attacking";
            print("New state: " + state);
            damageDelay = 0;
        }


        // Update state
        switch (state)
        {
            case "chasing":
                Chasing();
                break;
            case "wandering":
                Wandering();
                break;
            case "fleeing":
                Wandering();
                break;
            case "attacking":
                Attacking();
                break;
        }
    }

    void Chasing()
    {
        Vector3 diffVec = player.transform.position - rigidBody.transform.position;
        moveDirection = diffVec.normalized;

        rigidBody.AddForce(moveDirection * moveForce * 10f, ForceMode.Force);
        if (rigidBody.velocity.magnitude >= maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }
    }

    void Attacking()
    {
        Chasing();
        if(damageDelay <= 0) {
            player.GetComponent<PlayerController>().DoDamage(attackDamage);
            damageDelay = damageTickRate;
        } else {
            damageDelay--;
        }
    }

    void Wandering()
    {
        float rand = Random.Range(0.0f, 1.0f);

        moveDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;

        if (rand < randomMovementFreq)
        {
            //print(moveDirection);
            //print("random movement");
            rigidBody.AddForce(moveDirection * randomMovementForce, ForceMode.Force);
            if (rigidBody.velocity.magnitude >= maxSpeed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
            }
        }
    }

}
