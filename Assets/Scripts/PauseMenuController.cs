using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PauseMenu pauseMenu;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.Pause();
        }
    }

    public void playClickSound()
    {
        audioSource.Play();
    }
}
