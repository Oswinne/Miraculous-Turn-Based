using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject languageMenu;
    public GameObject soundMenu;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
       pauseMenu.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                resumeGame();
            }
            else{
                pauseGame();
            }
       } 
    }


    public void pauseGame(){
        AudioManager.instance.pause();
        AudioManager.instance.stopDanceInPauseMenu();
        
        pauseMenu.SetActive(true);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        languageMenu.SetActive(false);
        soundMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void resumeGame(){
        AudioManager.instance.play();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
