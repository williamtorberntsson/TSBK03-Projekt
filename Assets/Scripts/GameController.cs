using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int cameraMode;
    private List<Vector3> cameraPositions;
    private List<Vector3> cameraAngles;
    private int points;

    [Header("Spawn piraja")]
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject pirajaPrefab;
    [SerializeField] private int nrOfPirajaToSpawn;
    private float spawnCountDown;

    [SerializeField] private int respawnRadius;
    private bool toggleCam;

    [SerializeField] private TMP_Text scoreText;

    private GameObject duck;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        cameraMode = 0;
        toggleCam = false;

        // Get duck gameobject
        duck = GameObject.FindGameObjectWithTag("Duck");

        spawnCountDown = spawnInterval;

        SpawnNewPirajas(nrOfPirajaToSpawn);

        cameraPositions = new List<Vector3>();
        cameraAngles = new List<Vector3>();


        // Camera angle 0
        cameraPositions.Add(new Vector3(0.0f, 16.7f, -14.7f));
        cameraAngles.Add(new Vector3(55f, 0.0f, 0.0f));
        // Camera angle 1
        cameraPositions.Add(new Vector3(0.0f, 7.60f, -15.85f));
        cameraAngles.Add(new Vector3(25.8f, 0.0f, 0.0f));
        setCameraMode(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            switchCamera();
        }

        // Update spawn timer
        spawnCountDown -= Time.deltaTime;
        if (spawnCountDown <= 0)
        {
            SpawnNewPirajas(nrOfPirajaToSpawn);
            spawnCountDown = spawnInterval;
        }
    }

    void setCameraMode(int mode)
    {
        Camera.main.transform.position = cameraPositions[mode];
        Camera.main.transform.eulerAngles = cameraAngles[mode];
    }

    void switchCamera()
    {
        int nrOfCameraModes = cameraPositions.Count;
        Camera.main.transform.position = cameraPositions[(cameraMode + 1) % nrOfCameraModes];
        Camera.main.transform.eulerAngles = cameraAngles[(cameraMode + 1) % nrOfCameraModes];
        cameraMode = (cameraMode + 1) % nrOfCameraModes;
    }

    public void AddPoints(int i)
    {
        points += i;
        scoreText.text = "SCORE: " + points;
        print("Points: " + points);
    }

    public int GetPoints()
    {
        return points;
    }

    private void SpawnNewPirajas(int n)
    {
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
        Instantiate(pirajaPrefab, new Vector3(respawnRadius * x, 7, respawnRadius * z), Quaternion.identity);
    }

    public void DuckDied()
    {
        duck.GetComponent<DuckController>().KillDuck();
        StartCoroutine(ReloadInSecs(10.0f));
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
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
