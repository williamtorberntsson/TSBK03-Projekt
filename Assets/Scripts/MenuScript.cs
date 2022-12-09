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


    IEnumerator ReloadInSecs(float t)
    {
        yield return new WaitForSeconds(t);
        gameController.GetComponent<GameController>().enabled = true;
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
}
