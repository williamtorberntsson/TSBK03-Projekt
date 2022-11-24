using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DuckController : MonoBehaviour
{
    private int health;
    private bool controlsEnabled;
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

    Stack<GameObject> inactiveDucks;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        controlsEnabled = true;
        List<GameObject> children = new List<GameObject>();
        inactiveDucks = new Stack<GameObject>();

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
        if(controlsEnabled){
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
        print("Gained " + gainHealthAmount + " health");
        health += gainHealthAmount;
        GameObject currDuck = inactiveDucks.Pop();
        currDuck.tag = "ActiveLife";
        currDuck.SetActive(true);
    }

    public void DoDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        if (health <= 0)
        {
            health = 0;
            KillDuck();
            return;
        }

        GameObject[] ducks = GameObject.FindGameObjectsWithTag("ActiveLife");
        inactiveDucks.Push(ducks[0]);
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

    void ReleasePiraja()
    {
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

    IEnumerator ReloadInSecs(float t)
    {

            yield return new WaitForSeconds(t);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name   );
    }
    void KillDuck()
    {
        GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None; 
        controlsEnabled = false;
        GetComponentInParent<PlayerMovement>().controlsEnabled = false;
        ReloadInSecs(1.0f);
        
        
    }
}
