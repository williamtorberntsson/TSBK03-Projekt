using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool toggleCam;
    private int points;
    void Start()
    {
        points = 0;
        toggleCam = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
        if (Input.GetKeyDown(KeyCode.K)){
            if(toggleCam){
                Camera.main.transform.position = new Vector3(0.0f,16.7f,-14.7f);
                Camera.main.transform.eulerAngles = new Vector3(55f, 0.0f, 0.0f);    
                toggleCam = false;
            }
            else{
                Camera.main.transform.position = new Vector3(0.0f,7.60f,-15.85f);
                Camera.main.transform.eulerAngles = new Vector3(25.8f, 0.0f, 0.0f);
                toggleCam = true;   
            }
        }
        
    }

    public void AddPoints(int i){
        points += i;
        print("Points: " + points);
    }

    public int GetPoints(){
        return points;
    }
}
