using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject cam;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator ReloadInSecs(float t)
    {
        yield return new WaitForSeconds(t);
        gameController.GetComponent<GameController>().enabled = true;
    }
    public void StartGame(){
        print("CLICKED!");
        cam.GetComponent<Animator>().enabled = true;
        StartCoroutine(ReloadInSecs(2.0f));

    }
}
