using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.SimpleLocalization;

public class MainMenu : MonoBehaviour
{

    public void playGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.activateSound("battleSound");
    }

    public void replay(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.activateSound("battleSound");

    }
    
    public void backToMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        AudioManager.instance.activateSound("menuSound");


    }


    public void quitGame(){
        Application.Quit();
    }



    public void selectFrench(){
        LocalizationManager.Read();
        LocalizationManager.Language = "French";
        PlayerPrefs.SetString("language","French");
    }


    public void selectEnglish(){
        LocalizationManager.Read();
        LocalizationManager.Language = "English";
        PlayerPrefs.SetString("language","English");
    }

    public void resumeGame(){
        var pauseMenu = GameObject.Find("ActionMenuCanvas").GetComponent<PauseMenu>();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        pauseMenu.isPaused = false;
        AudioManager.instance.play();
    }


}
