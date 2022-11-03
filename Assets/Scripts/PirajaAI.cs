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

    [Header("Fleeing")]
    [SerializeField] private float fleeAngle;
    [SerializeField] private float fleeRange;



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
        if(state != "caught"){
                
            if (state != "chasing" && distanceToPlayer < sightRange && distanceToPlayer > attackRange)
            {
                state = "chasing";
                print("New state: " + state);
            }
            //loses sight
            if ((state == "chasing" || state == "fleeing") && distanceToPlayer > sightRange)
            {
                state = "wandering";
                print("New state: " + state);
            }

            if ((state == "chasing" || state == "fleeing") && state != "attacking" && distanceToPlayer < attackRange)
            {
                state = "attacking";
                print("New state: " + state);
                damageDelay = 0;
            }

            if (state != "fleeing" && DoesPlayerSeeFish() && distanceToPlayer < fleeRange) {
                state = "fleeing";
                print("New state: " + state);
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
                break;
        }
    }

    public void setCaught(){
        state = "caught";
        print("New state: " + state);
    }

    void Chase()
    {
        Vector3 diffVec = player.transform.position - rigidBody.transform.position;
        moveDirection = diffVec.normalized;
        SwimInDirection(moveDirection);
    }

    void Flee()
    {
        Vector3 diffVec = rigidBody.transform.position - player.transform.position;
        moveDirection = diffVec.normalized;
        SwimInDirection(moveDirection);
    }

    void Attack()
    {
        Chase();
        if (damageDelay <= 0)
        {
            player.GetComponent<DuckController>().DoDamage(attackDamage);
            damageDelay = damageTickRate;
        }
        else
        {
            damageDelay--;
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
            rigidBody.AddForce(moveDirection * randomMovementForce, ForceMode.Force);
            if (rigidBody.velocity.magnitude >= maxSpeed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
            }
        }
    }

    void SwimInDirection(Vector3 direction)
    {
        rigidBody.AddForce(direction * moveForce * 10f, ForceMode.Force);
        if (rigidBody.velocity.magnitude >= maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }
    }

    bool DoesPlayerSeeFish() {
        Vector3 PlayerToPirajaVec = rigidBody.transform.position - player.transform.position;


        float angle = Mathf.Acos( Vector3.Dot(PlayerToPirajaVec.normalized, player.transform.forward));
        //print("angle: " +angle + " < " + fleeAngle* Mathf.PI / 180);

         

        return (angle < fleeAngle* Mathf.PI / 180);
    }

}
