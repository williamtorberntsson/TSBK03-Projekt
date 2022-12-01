using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    private int health;
    private bool controlsEnabled;
    private GameObject gameController;
    private GameObject beak;
    private GameObject caughtPiraja;
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int gainHealthAmount;

    [Header("Catch/throw")]
    [SerializeField] private float catchRange;
    [SerializeField] private float throwHeight;
    [SerializeField] private float throwForce;
    [Header("Controls")]
    [SerializeField] private KeyCode catchKey = KeyCode.Space;

    [Header("Smoke")]
    [SerializeField] private GameObject smoke;
    Stack<GameObject> inactiveDucks;
    Stack<GameObject> activeDucks;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        controlsEnabled = true;
        List<GameObject> children = new List<GameObject>();
        inactiveDucks = new Stack<GameObject>();
        activeDucks = new Stack<GameObject>();
        gameController = GameObject.FindGameObjectWithTag("GameController");

        // Push all lives to activeDucks stack
        GameObject[] allDucks = GameObject.FindGameObjectsWithTag("Life");
        for(int i = allDucks.Length - 1; i >= 0 ; i--) {
            activeDucks.Push(allDucks[i]);
        }

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
        if (controlsEnabled)
        {
            AimControls();

            // Catch/Release piraja
            if (Input.GetKeyDown(catchKey))
            {
                if (caughtPiraja)
                {
                    ReleasePiraja();
                }
                else
                {
                    CatchPiraja();
                }
            }
        }
    }

    public bool hasCaughtPiraja()
    {
        return caughtPiraja;
    }

    public void giveHealth()
    {
        if(health < maxHealth){
            print("Gained " + gainHealthAmount + " health");

            health += gainHealthAmount;
            GameObject currDuck = inactiveDucks.Pop();
            Instantiate(smoke, currDuck.transform.position, currDuck.transform.rotation);
            activeDucks.Push(currDuck);
            currDuck.SetActive(true);
        }
    }

    public void DoDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            GameObject currDuck = activeDucks.Pop();
            Instantiate(smoke, currDuck.transform.position, currDuck.transform.rotation);
            inactiveDucks.Push(currDuck);
            currDuck.SetActive(false);
            print("I took " + damage + " damage!");
        
            if (health <= 0)
            {
                health = 0;
                gameController.GetComponent<GameController>().DuckDied();
                return;
            }
        }
    }

    void AimControls()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.tag != "Player")
        {
            Vector3 lookPos = hit.point;
            transform.LookAt(lookPos);
            }
    }

    void ReleasePiraja()
    {
        Vector3 throwDir = new Vector3(transform.forward.x, 0, transform.forward.z);
        throwDir = Vector3.Normalize(throwDir);
        throwDir.y = throwHeight;   // OM Y ÄR HÖGT SÅ BLIR Z OCH X LÅGA. NORMALISERA X OCH Z!!!! SUMMA = 1
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

        Debug.DrawRay(startPoint, endPoint * rayLength, Color.magenta);
        // If we hit piraja
        if (Physics.Raycast(startPoint, endPoint, out hit, rayLength))
        { //raycast is low an strait
            if (hit.transform.gameObject.tag == "Piraja")
            {
                print("HIT PIRAJA!!!");
                hit.transform.GetComponent<PirajaAI>().SetCaught(beak);
                caughtPiraja = hit.transform.gameObject;
            }
        }
        // move piraja to nose
    }


    public void KillDuck()
    {
        GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None;
        controlsEnabled = false;
        GetComponentInParent<PlayerMovement>().controlsEnabled = false;
    }
}
