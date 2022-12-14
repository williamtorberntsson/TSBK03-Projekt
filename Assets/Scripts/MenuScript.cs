using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject cam;

    [SerializeField] private Texture2D cursorTexture;
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
        print("prev state:" + state);
        if(state == "start") {
            animator.SetTrigger("onStartFromStart");
            state = "game";
            GetComponent<AudioSource>().Play();
            StartCoroutine(ReloadInSecs(5.3f));
        } else if(state == "controls") {
            animator.SetTrigger("onControlsToStartGame");
            state = "game";
            GetComponent<AudioSource>().Play();
            StartCoroutine(ReloadInSecs(5.5f));
        }
        MovePlayerToKitchen();
    }

    public void ControlsButton() {
        print("prev state:" + state);
        if(state == "start") {
            animator.SetTrigger("onStartToControls");
            state = "controls";
            GetComponent<AudioSource>().Play();
        }
    }

    private void MovePlayerToKitchen() {
        print("moving player to kitchen");
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<Transform>().transform.position = new Vector3(-9, 3, -3);
    }

    IEnumerator ReloadInSecs(float t)
    {
        yield return new WaitForSeconds(t);
        gameController.GetComponent<GameController>().enabled = true;
    }

}
