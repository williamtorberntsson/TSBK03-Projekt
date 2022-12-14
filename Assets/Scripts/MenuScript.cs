using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject cam;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private string animationTriggerName;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    private string state = "start";
    private Animator animator;

    void Start()
    {
        animator = cam.GetComponent<Animator>();
        animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGameButton() {
        if(state == "start") {
            animator.SetTrigger("onStartFromStart");
            state = "game";
            GetComponent<AudioSource>().Play();
            StartCoroutine(ReloadInSecs(3.0f));
        } else if(state == "controls") {
            animator.SetTrigger("onControlsToStartGame");
            state = "game";
            GetComponent<AudioSource>().Play();
            StartCoroutine(ReloadInSecs(4.0f));
        }
    }

    public void ControlsButton() {
        if(state == "start") {
            animator.SetTrigger("onStartToControls");
            state = "controls";
            GetComponent<AudioSource>().Play();
        }
    }

    public void StartGame()
    {
        print("CLICKED!");
        GetComponent<AudioSource>().Play();
        //cam.GetComponent<Animator>().enabled = true;
        print("Trigger: " + animationTriggerName);

        // This
        // animator.SetTrigger(animationTriggerName); does not work
        animator.SetTrigger("onStartFromStart"); // works
        // This

        StartCoroutine(ReloadInSecs(2.0f));
        
        // Set cursor
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        //Cursor.visible = true;
    }

    IEnumerator ReloadInSecs(float t)
    {
        yield return new WaitForSeconds(t);
        gameController.GetComponent<GameController>().enabled = true;
    }

}
