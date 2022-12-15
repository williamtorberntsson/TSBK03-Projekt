using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameController;

    private bool paused;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        paused = false;
        animator = Camera.main.GetComponent<Animator>();
    }
    public void Pause() {
        if(!paused){
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
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
