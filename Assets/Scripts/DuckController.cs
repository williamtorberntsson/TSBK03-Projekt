using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    private int health;
    private GameObject beak;
    private GameObject caughtPiraja;
    [SerializeField] private int maxHealth;

    [Header("Catch/throw")]
    [SerializeField] private float catchRange;
    [SerializeField] private float throwHeight;
    [SerializeField] private float throwForce;
    [Header("Controls")]
    [SerializeField] private KeyCode catchKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        List<GameObject> children = new List<GameObject>();

        // Find beak object
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform currentItem = transform.GetChild(i);
            //Search by name
            if (currentItem.name.Equals("Beak"))
            {
                beak = currentItem.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AimControls();

        // Catch/Release piraja
        if (Input.GetKeyDown(catchKey))
        {
            if(caughtPiraja){
                ReleasePiraja();
            }
            else{
                CatchPiraja();
            }
        }
    }

    public bool hasCaughtPiraja(){
        return caughtPiraja;
    }

    public void giveHealth(int healthPoint){
        health += healthPoint;
    }

    public void DoDamage(int damage)
    {
        if(health > 0){
            health -= damage;
            if(health < 0){
                health = 0;
            }
        }

        GameObject[] ducks = GameObject.FindGameObjectsWithTag("ActiveLife");

        ducks[0].tag = "InactiveLife";
        ducks[0].SetActive(false);

        print("I took " + damage + " damage!");
    }

    void AimControls()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.tag != "Player")
        {
            transform.LookAt(hit.point);
        }
    }

    void ReleasePiraja(){
        Vector3 throwDir = new Vector3(transform.forward.x, throwHeight, transform.forward.z);
        caughtPiraja.GetComponent<PirajaAI>().SetReleased(throwDir, throwForce);
        caughtPiraja = null;
    }

    void CatchPiraja()
    {
        // is paraja infront
        RaycastHit hit;
        float pirajaHeight = 0.5f;
        float rayLength = catchRange;

        Vector3 startPoint = new Vector3(transform.position.x, pirajaHeight, transform.position.z); //duck pos
        Vector3 endPoint = new Vector3(transform.forward.x, 0, transform.forward.z);

        Debug.DrawRay(startPoint, endPoint * rayLength, Color.green);
        // If we hit piraja
        if (Physics.Raycast(startPoint, endPoint, out hit, rayLength))
        { //raycast is low an strait
            if (hit.transform.gameObject.tag == "Piraja")
            {
                hit.transform.GetComponent<PirajaAI>().SetCaught(beak);
                caughtPiraja = hit.transform.gameObject;
            }


        }
        // move piraja to nose
    }
}
