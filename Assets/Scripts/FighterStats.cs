using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats : MonoBehaviour, IComparable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    public GameObject specialParticles;

    [SerializeField]
    public GameObject cataAniamtionParticlesBack;

    [SerializeField]
    public GameObject cataAniamtionParticlesFront;


    
    [SerializeField]
    public GameObject cataExplosion;

    
    [SerializeField]
    public GameObject cataExplosion2;

    [SerializeField]
    public GameObject teamworkParticles;


    [SerializeField]
    public GameObject superCataCharge;
    [SerializeField]
    public GameObject superCataLaunch;


    [SerializeField]
    public GameObject megaCataCharge;
    [SerializeField]
    public GameObject megaCataLaunch;
    
    
    
    [Header("Stats")]
    public int health;
    public int numOfHearts;
    public int dodge;
    public int speed;
    private int startHealth;

    private bool dead = false;




    [HideInInspector]
    public int nextActTurn;
    // private bool dead = false;

    // Resize health bar


    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool isSpecialActivated = false;
    public bool isTauntActivated = false;

    public bool isSpecialUsed = false;

    public bool isTeamWorkActivated = false;

    public GameObject victim;

    public bool hit = false;
    

    private void Awake()
    {
        startHealth = health;

    }


    public void receiveDamage(int damage){

        health = health - damage;
        
        // animator.Play("Damage");

        // Set Damage text

        if(health <= 0 ){
            dead = true;
            var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
            gameObject.GetComponent<FadeOut>().startFading();

            if(damage != 50){
                if(gameObject.name == "Ladybug"){
                    gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("deathLady");
                }
                else{
                    gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("deathChat");
                    specialParticles.SetActive(false);
            }
            }

        }
        
        if(health > numOfHearts){
            health = numOfHearts;
        }
        for(int i = 0; i < hearts.Length; i++){

            if(i < health){
                hearts[i].sprite = fullHeart;
            }
            else{
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
        


    }



    public int CompareTo(object otherStats){
        // Will be used for speed comparison
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn);
        return nex;
    }

    public bool GetDead(){

        return dead;
    }

    public void ContinueGame(){
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }

    public void continueAfter1Sec(){
        Invoke("ContinueGame",1);
    }

    public void CalculateNextTurn(int currentTurn){
        nextActTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }


    public void AlertObservers(string message)
    {
        var victimFighterStats = this;
        if(victim != null){
            victimFighterStats = victim.GetComponent<FighterStats>();
        }
        if(gameObject.name == "ChatNoir" && message != "ReloadAnimationEnded" && isSpecialActivated == true && message != "impact" && message != "cataclysmAttack" && !isSpecialUsed){
            specialParticles.SetActive(true);
        }
       
        switch (message)
        {

            case "AttackAnimationEnded" :
                 
            break;
            case "ReloadAnimationEnded":
                gameObject.SetActive(false);
                var currentUserStats = gameObject.GetComponent<FighterStats>();
                currentUserStats.receiveDamage(-2);
                var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
                gameController.teamWorkButton.GetComponent<Button>().interactable = false;
            break;
            case "SpecialAnimationEnded":
                isSpecialActivated = true;
            break;
            case "SneakAnimationEnded":
            break;
            case "PunAnimationEnded":
            break;
            case "MladyEnded":
                AudioManager.instance.stopSFX();
            break;
            case"TeamworkAnimationEnded":
                 var currentUserAction = gameObject.GetComponent<FighterAction>();
                 currentUserAction.chatActive();
                 teamworkParticles.SetActive(false);
                break;
            case"catAnimation":
                cataAniamtionParticlesBack.SetActive(true);
                cataAniamtionParticlesFront.SetActive(true);
                Invoke("stopAnimation",3);
            break;




            case "chatBlancMidAttack1":
            case "chatBlancMidAttack2":
                if(hit){
                    
                    if(victimFighterStats.dodge == 100 ){
                        victimFighterStats.dodge = 0; 
                        victim.GetComponent<FighterAction>().selectAttack("dodge");
                    }else{
                        victimFighterStats.receiveDamage(1);
                        damageAnimationHero();
                    }
                    hit = false;
                }

            break;

            case "impact":
                if(hit){
                    victim.GetComponent<ChatBlancAction>().takeHits(gameObject);
                    var victimFightAction = victim.GetComponent<ChatBlancAction>();
                    victimFightAction.damagePrefab.GetComponent<AttackScriptChatBlanc>().damageAnimation();
                    hit = false;
                }

            break;

            case"SuperCataLaunchEnded":
                superCataLaunch.SetActive(false);
                animator.SetLayerWeight(2,0);
                var animation = victim.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
                if(victimFighterStats.dodge == 100 ){
                    victimFighterStats.dodge = 0; 
                    victim.GetComponent<FighterAction>().selectAttack("dodge");
                }
                else if(animation[0].clip.name.Contains("Block")){
                    victimFighterStats.receiveDamage(2);
                    damageAnimationHero();
                    AudioManager.instance.activateSoundAttack("superCataExplosion");    
                }
                else{
                    victimFighterStats.receiveDamage(4);
                    damageAnimationHero();
                    AudioManager.instance.activateSoundAttack("superCataExplosion");    
                }

            break;

            case"cataclysmAttack":
                if(hit){
                    var victimFightAction = victim.GetComponent<ChatBlancAction>();
                    if(victimFightAction.superCataActive){
                        cataExplosion.SetActive(true);
                    }
                    else if(victimFightAction.megaCataActive){
                        var gameController2 = GameObject.Find("GameControllerObject").GetComponent<GameController>();
                        if(!victimFightAction.firstLock){
                            gameController2.dialogueBox.GetComponent<Dialogue>().restartDialoge("CataInMega1");
                        }
                        else{
                            gameController2.dialogueBox.GetComponent<Dialogue>().restartDialoge("CataInMega2");
                        }
                        cataExplosion2.SetActive(true);
                    }
                    else{
                        cataExplosion2.SetActive(true);
                    }
                    victim.GetComponent<ChatBlancAction>().takeHits(gameObject);
                    victimFightAction.damagePrefab.GetComponent<AttackScriptChatBlanc>().damageAnimation();
                    Invoke("stopAnimation",3);
                    hit = false;
                }
                isSpecialUsed = true;
            break;


            default:
            break;
        }
        if(message!="catAnimation" && !message.Contains("chatBlancMidAttack") && message!="BackWardEnded" && message!="impact" && message!="dodgeEnded"){
            continueAfter1Sec();
            
        }

    }

    public void AlertObserversSFX(string message){

         switch (message)
        {
            case "SFXChatBlancAttack":
                if(hit){
                    AudioManager.instance.activateSoundAttack(gameObject.name);
                }
                else{
                    AudioManager.instance.activateSoundAttack("block");
                }
            break;

            case "SFXAttack":
                    AudioManager.instance.activateSoundAttack(gameObject.name);
            break;


            case "ChatNoirReload":
            case "SneakStart":
            case "Sneakhit":
            case "teamwork":
            case "purr":
            case "cataPunchLoad":
            case "heavyPunch":
            case "reloadPart1":
            case "reloadPart2":
                if(!GetDead()){
                    AudioManager.instance.activateSoundAttack(message);
                }
            break;

        }
    }

    private void damageAnimationHero()
    {
        if (hit)
        {

            var victimFightAction = victim.GetComponent<FighterAction>();
            victimFightAction.damagePrefab.GetComponent<AttackScript>().damageAnimation();
            hit = false;

        }
    }

    private void stopAnimation(){
        cataAniamtionParticlesBack.SetActive(false);
        cataAniamtionParticlesFront.SetActive(false);
        cataExplosion.SetActive(false);
        cataExplosion2.SetActive(false);
    }

}
