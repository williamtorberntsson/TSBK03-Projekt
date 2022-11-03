using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    private int health;
    private GameObject beak;
    [SerializeField] private int maxHealth; 
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;    
        List<GameObject> children = new List<GameObject>();
 
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
        CatchPiraja();
    }

    public void DoDamage(int damage) {
        health -= damage;
        print("I took " + damage + " damage!");
    }

    void AimControls() {
        var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100) && hit.transform.tag != "Player" ) 
        {
            transform.LookAt(hit.point);
        }
    }

    void CatchPiraja() {
        // is paraja infront
        RaycastHit hit;
        float pirajaHeight = 0.5f;

        // If we hit piraja

        Debug.DrawRay(new Vector3(transform.position.x, pirajaHeight, transform.position.z), new Vector3(transform.forward.x, pirajaHeight, transform.forward.z), Color.green);
        if( Physics.Raycast(new Vector3(transform.position.x, pirajaHeight, transform.position.z), new Vector3(transform.forward.x, pirajaHeight, transform.forward.z), out hit, 100)) { //raycast is low an strait
            if(hit.transform.GetComponent<PirajaAI>()){
                hit.transform.GetComponent<PirajaAI>().setCaught();
                hit.transform.position = beak.transform.position ;
            }


        }
        // move piraja to nose
    }
}
