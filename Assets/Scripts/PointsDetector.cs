using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDetector : MonoBehaviour
{

    [Header("Health")]
    private GameObject gameController, duckController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        duckController = GameObject.FindGameObjectWithTag("Duck");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        gameController.GetComponent<GameController>().AddPoints(1);
        duckController.GetComponent<DuckController>().giveHealth();
        GetComponent<AudioSource>().Play();
        collision.gameObject.tag = "Point";
        print("setting tag to point!");
    }
}
