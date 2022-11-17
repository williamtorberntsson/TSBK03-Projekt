using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private int cameraMode;
    private List<Vector3> cameraPositions;
    private List<Vector3> cameraAngles;
    private int points;

    private bool toggleCam;

    [SerializeField] private TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        cameraMode = 0;
        toggleCam = false;

        cameraPositions = new List<Vector3>();
        cameraAngles = new List<Vector3>();

        // Camera angle 0
        cameraPositions.Add(new Vector3(0.0f,16.7f,-14.7f));
        cameraAngles.Add(new Vector3(55f, 0.0f, 0.0f));

        // Camera angle 1
        cameraPositions.Add(new Vector3(0.0f,7.60f,-15.85f));
        cameraAngles.Add(new Vector3(25.8f, 0.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
        if (Input.GetKeyDown(KeyCode.K)){
            switchCamera();
        }
    }

    void setCameraMode(int mode) {
        Camera.main.transform.position = cameraPositions[mode];
        Camera.main.transform.eulerAngles = cameraAngles[mode];
    }

    void switchCamera() {
        int nrOfCameraModes = cameraPositions.Count;
        Camera.main.transform.position = cameraPositions[(cameraMode + 1) % nrOfCameraModes];
        Camera.main.transform.eulerAngles = cameraAngles[(cameraMode + 1) % nrOfCameraModes];
        cameraMode = (cameraMode + 1) % nrOfCameraModes;
    }

    public void AddPoints(int i){
        points += i;
        scoreText.text = "SCORE: " + points;
        print("Points: " + points);
    }

    public int GetPoints(){
        return points;
    }
}
