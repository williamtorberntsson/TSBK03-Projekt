using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject backgroundMusicController;

    [SerializeField] private Texture2D cursorTexture;
    private string state;
    private Animator animator;
    private GameObject Player;

    void Start()
    {
        state = "start";
        animator = cam.GetComponent<Animator>();
        animator.enabled = true;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGameButton() {
        print("prev state:" + state);
        if(state == "start") {
            StartGame(0.07f, "onStartFromStart");
        } else if(state == "controls") {
            StartGame(0f, "onControlsToStartGame");
        }
    }

    private void StartGame(float delay, string trigger)
    {
        animator.SetTrigger(trigger);
        state = "game";
        GetComponent<AudioSource>().Play();
        StartCoroutine(MakeSoundNotMuffled(delay));
        StartCoroutine(MovePlayerToKitchen(delay));
        StartCoroutine(StartGameInSecs(5f + delay));
    }

    public void ControlsButton() {
        print("prev state:" + state);
        if(state == "start") {
            animator.SetTrigger("onStartToControls");
            state = "controls";
            GetComponent<AudioSource>().Play();
        }
    }
    IEnumerator MakeSoundNotMuffled(float delay)
    {
        yield return new WaitForSeconds(2.8f + delay);
        backgroundMusicController.GetComponent<AudioReverbFilter>().enabled = false;

    }

    IEnumerator MovePlayerToKitchen(float delay) {
        print("moving player to kitchen");
        Player.GetComponentInChildren<DuckController>().enabled = false;
        yield return new WaitForSeconds(4.5f + delay);
        Player.GetComponent<Transform>().transform.position = new Vector3(-9, 3, -3);
    }

    IEnumerator StartGameInSecs(float t)
    {
        yield return new WaitForSeconds(t);
        gameController.GetComponent<GameController>().enabled = true;
        Player.GetComponentInChildren<DuckController>().enabled = true;
    }

}
