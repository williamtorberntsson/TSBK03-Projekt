using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject backgroundMusicController;

    private bool paused;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        paused = true;
        animator = Camera.main.GetComponent<Animator>();
    }
    public void Pause() {
        gameController.playClickSound();
        if (!paused){
            backgroundMusicController.GetComponent<AudioSource>().pitch = 0.6f;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
        else{
            Resume();
            paused = false;
        }
        
    }
    public void Resume() {
        gameController.playClickSound();
        backgroundMusicController.GetComponent<AudioSource>().pitch = 1f;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    public void Restart() {
        gameController.playClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void ExitGame() {
        gameController.playClickSound();
        Application.Quit();
    }

    public void ChangeDifficulty(float value){
        print("changed diffi: " + value);
        gameController.setDifficulty(value);
    }
}
