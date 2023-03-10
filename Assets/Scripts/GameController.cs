using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int cameraMode;
    private List<Vector3> cameraPositions;
    private List<Vector3> cameraAngles;
    private int points;

    private Animator animator;
    [SerializeField] private GameObject cam;


    [Header("Spawn piraja")]
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject pirajaPrefab;
    [SerializeField] private int minNrOfPirajaToSpawn = 3;
    [SerializeField] private int spawnScaling = 3;
    private float spawnCountDown;
    [SerializeField] private float forceHeight;
    [SerializeField] private float spawnForce;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private float collisionDelay;
    [Header("Audio")]
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private AudioClip plopSound;
    [SerializeField] private AudioClip popSound;
    [SerializeField] private AudioClip biteSound;
    [SerializeField] private AudioClip clickSound;


    private bool shouldPlayBiteSound;

    [SerializeField] private Text scoreText;

    private GameObject duck;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        cameraMode = 0;
        shouldPlayBiteSound = false;
        
        // Get duck gameobject
        duck = GameObject.FindGameObjectWithTag("Duck");

        spawnCountDown = spawnInterval;

        audioSource = GetComponent<AudioSource>();

        SpawnNewPirajas(minNrOfPirajaToSpawn);

        cameraPositions = new List<Vector3>();
        cameraAngles = new List<Vector3>();

        animator = cam.GetComponent<Animator>();
        animator.enabled = true;


        // Camera angle 0
        cameraPositions.Add(new Vector3(0.0f, 16.7f, -13.6f));
        cameraAngles.Add(new Vector3(55f, 0.0f, 0.0f));
        // Camera angle 1
        cameraPositions.Add(new Vector3(0.0f, 7.60f, -15.85f));
        cameraAngles.Add(new Vector3(25.8f, 0.0f, 0.0f));
        setCameraMode(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            print("switch camera");
            switchCamera();
        }

   
        if (shouldPlayBiteSound)
        {
            audioSource.clip = biteSound;
            audioSource.Play();
            shouldPlayBiteSound = false;
        }

        // Update spawn timer
        spawnCountDown -= Time.deltaTime;
        if (spawnCountDown <= 0)
        {
            if (points / spawnScaling <= minNrOfPirajaToSpawn)
            {
                SpawnNewPirajas(minNrOfPirajaToSpawn);
            }
            else
            {
                SpawnNewPirajas(points / spawnScaling);
            }
            spawnCountDown = spawnInterval;
        }
    }

    public void setDifficulty(float newSpawnScaling){
        spawnScaling = (int)newSpawnScaling;
    }

    public void playBiteSound()
    {
        if (!shouldPlayBiteSound)
        {
            shouldPlayBiteSound = true;
        }
    }

   
    void setCameraMode(int mode)
    {
        Camera.main.transform.position = cameraPositions[mode];
        Camera.main.transform.eulerAngles = cameraAngles[mode];
    }

    void switchCamera()
    {
        animator.enabled = false;

        int nrOfCameraModes = cameraPositions.Count;
        Camera.main.transform.position = cameraPositions[(cameraMode + 1) % nrOfCameraModes];
        Camera.main.transform.eulerAngles = cameraAngles[(cameraMode + 1) % nrOfCameraModes];
        cameraMode = (cameraMode + 1) % nrOfCameraModes;
    }

    public void AddPoints(int i)
    {
        points += i;
        scoreText.text = "Score: " + points;

        print("Points: " + points);

        if (points % 10 == 0)
        {
            ClearPot();
        }
    }

    private void ClearPot()
    {
        GameObject[] pointPirajas = GameObject.FindGameObjectsWithTag("Point");
        print("Clearing pirajas! nr of them: " + pointPirajas.Length);
        for (int i = 0; i < pointPirajas.Length; i++)
        {
            Destroy(pointPirajas[i]);
        }
    }

    public int GetPoints()
    {
        return points;
    }

    private void SpawnNewPirajas(int n)
    {
        audioSource.clip = plopSound;
        audioSource.Play();
        for (int i = 0; i < n; i++)
        {
            SpawnNewPiraja();
        }
    }

    private void SpawnNewPiraja()
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);
        GameObject newPiraja = Instantiate(pirajaPrefab, spawnPos, Quaternion.identity);
        newPiraja.GetComponentInChildren<Collider>().enabled = false;
        newPiraja.GetComponentInChildren<PirajaAI>().SetState("flying");
        StartCoroutine(TurnOnCollision(newPiraja));
        newPiraja.GetComponent<Rigidbody>().AddForce(new Vector3(x * spawnForce, forceHeight, z * spawnForce));
    }

    public void DuckDied()
    {
        duck.GetComponent<DuckController>().KillDuck();
        StartCoroutine(ReloadInSecs(7.0f));
        PirajaEatDeadDuck();
    }

    public void PirajaEatDeadDuck()
    {
        GameObject[] pirajas = GameObject.FindGameObjectsWithTag("Piraja");
        for (int i = 0; i < pirajas.Length; i++)
        {
            pirajas[i].GetComponent<PirajaAI>().SetState("deadDuck");
        }
    }

    IEnumerator ReloadInSecs(float t)
    {
        yield return new WaitForSeconds(2.0f);
        audioSource.clip = deadSound;
        audioSource.Play();
        yield return new WaitForSeconds(t - 2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator TurnOnCollision(GameObject piraja)
    {
        yield return new WaitForSeconds(collisionDelay);
        piraja.GetComponentInChildren<Collider>().enabled = true;
    }
}
