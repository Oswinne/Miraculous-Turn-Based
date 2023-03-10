using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int currentTimer = 0;
    private int startingTimer = 3;

    public Text text;
    void Start()
    {
        currentTimer = startingTimer;
        
    }


    public void decrease(){
        currentTimer--;
        text.text = currentTimer.ToString();
    }

    public void reset(){

        currentTimer = startingTimer;
        text.text = currentTimer.ToString();
        var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        gameController.teamWorkTimer = 3;
    }

    public void setTimer(int timer){
         text.text = timer.ToString();
    }


}
