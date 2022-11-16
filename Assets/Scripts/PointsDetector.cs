using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDetector : MonoBehaviour
{

    private GameObject gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectsWithTag("GameController")[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        gameController.GetComponent<GameController>().AddPoints(1);

    }
}
