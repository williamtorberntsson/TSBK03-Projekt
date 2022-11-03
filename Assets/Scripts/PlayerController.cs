using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth; 
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDamage(int damage) {
        health -= damage;
        print("I took " + damage + " damage!");
    }
}
