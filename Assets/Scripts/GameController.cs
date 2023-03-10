using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<FighterStats> fighterStats;

    [SerializeField]
    private GameObject battleMenuLadyBug;

    [SerializeField]
    private GameObject battleMenuSpecialLadyBug;

    [SerializeField]
    private GameObject battleMenuChatNoir;

    [SerializeField]
    private GameObject battleMenuSpecialChatNoir;

    [SerializeField]
    private GameObject cataButton;

    [SerializeField]
    private GameObject luckyCharmButton;

    [SerializeField]
    public GameObject teamWorkButton;

    [SerializeField]
    public GameObject dialogueBox;

    private int howManyPlayed = 0;
    private int turn = 0;

    private int lbReloadTimer = 0;
    private int cnReloadTimer = 0;

    public int cataTimer { get; set; }  = 0;
    public int luckyTimer { get; set; }  = 0;

    public int punTimer = 0;

    public int teamWorkTimer = 3;

    public int numberOfCharactersInOneTurn = 3;

    private GameObject chatNoir;
    private GameObject ladyBug;

    private FighterStats currentLadyBugStats;
    private FighterStats currentChatNoirStats;
    private FighterStats currentBossStats;


    void Start(){
        fighterStats = new List<FighterStats>();
        ladyBug = GameObject.FindGameObjectWithTag("LadyBug");

        currentLadyBugStats = ladyBug.GetComponent<FighterStats>();
        currentLadyBugStats.CalculateNextTurn(0);
        fighterStats.Add(currentLadyBugStats);

        chatNoir  = GameObject.FindGameObjectWithTag("ChatNoir");
        currentChatNoirStats = chatNoir.GetComponent<FighterStats>();
        currentChatNoirStats.CalculateNextTurn(0);
        fighterStats.Add(currentChatNoirStats);



        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        currentBossStats = boss.GetComponent<FighterStats>();
        currentBossStats.CalculateNextTurn(0);
        fighterStats.Add(currentBossStats);


        fighterStats.Sort();
        this.battleMenuLadyBug.SetActive(false);
        this.battleMenuSpecialLadyBug.SetActive(false);

        this.battleMenuChatNoir.SetActive(false);
        this.battleMenuSpecialChatNoir.SetActive(false);
        NextTurn();
    }

    public void NextTurn(){

        
        if(!currentBossStats.GetDead() && !currentChatNoirStats.GetDead() && !currentLadyBugStats.GetDead()){

            
            FighterStats currentFighterStats = fighterStats[0];
            fighterStats.Remove(currentFighterStats);
            GameObject currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);

            var animation = currentUnit.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);

            if(currentUnit.tag == "LadyBug"){

                lbReloadTimer = reloading(currentFighterStats, currentUnit, animation,lbReloadTimer,"ReloadLadyBugBackward","IdleLadyBug",false);

                luckyTimer = special(currentUnit,luckyTimer,this.battleMenuLadyBug,this.battleMenuSpecialLadyBug,"ReloadLadyBug");

                if(lbReloadTimer == 0 && luckyCharmButton.GetComponent<Button>().interactable == false && luckyTimer == 0){
                    luckyCharmButton.GetComponent<Button>().interactable = true;
                }
                
                if(lbReloadTimer == 0 && teamWorkButton.GetComponent<Button>().interactable == false && chatNoir.activeSelf && !ladyBug.GetComponent<FighterStats>().isTeamWorkActivated){
                    teamWorkButton.GetComponent<Button>().interactable = true;
                }

                calculateTeamworkTimer(currentUnit);
            }


            else if(currentUnit.tag == "ChatNoir")
            {

        
                cnReloadTimer = reloading(currentFighterStats, currentUnit, animation,cnReloadTimer,"ChatReloadBackward","ChatIdle",true);
    
                cataTimer = special(currentUnit,cataTimer,this.battleMenuChatNoir,this.battleMenuSpecialChatNoir,"ChatReload");


                if(cnReloadTimer == 0 && cataButton.GetComponent<Button>().interactable == false && cataTimer == 0){
                    cataButton.GetComponent<Button>().interactable = true;
                }


                calculateTauntTimer(currentUnit);

            }
            else
            {
                
               
                int randomCapacity = Random.Range(0, 9);
                var chatBlancAction = currentUnit.GetComponent<ChatBlancAction>();

                if(chatBlancAction.turnSkip){
                    chatBlancAction.turnSkip = false;
                    NextTurn();
                }
                else{
                    int superCataProba = 6;
                    int megaCataProba = 10;
                    bool atLeatOneActive = false;

                    atLeatOneActive = chatNoir.activeSelf || ladyBug.activeSelf;
                    
                    if(chatBlancAction.megaCataActive){
                        randomCapacity = megaCataProba;
                    }
                    else if(chatBlancAction.superCataActive){
                        randomCapacity = superCataProba;
                    }







                    if(randomCapacity < superCataProba && atLeatOneActive){
                        currentUnit.GetComponent<ChatBlancAction>().selectAttack("attack");
                    }
                    else if(randomCapacity < megaCataProba && atLeatOneActive){
                        
                    
                        if(chatBlancAction.superCataActive){
                            currentUnit.GetComponent<ChatBlancAction>().selectAttack("superCataLaunch");
                            chatBlancAction.superCataActive = false;
                        }
                        else{
                            currentUnit.GetComponent<ChatBlancAction>().selectAttack("superCata");
                            chatBlancAction.superCataActive = true;
                        }

                    }
                    else{
                        chatBlancAction.superCataActive = false;    
                        var animationBoss = currentUnit.GetComponent<Animator>();

                        if(chatBlancAction.megaCataActive){
                        
                            if(chatBlancAction.megaCataTimer == 2){
                                dialogueBox.GetComponent<Dialogue>().megaCataCast = true;
                                dialogueBox.GetComponent<Dialogue>().restartDialoge("MegaCataDamaged");
                                // currentUnit.GetComponent<ChatBlancAction>().selectAttack("megaCataLaunch");
                                chatBlancAction.megaCataActive = false;
                                chatBlancAction.megaCataTimer = 0;
                            }
                            else{
                                chatBlancAction.megaCataTimer++;
                                currentFighterStats.continueAfter1Sec();
                                
                            }
                        

                        }
                        else{
                            currentUnit.GetComponent<ChatBlancAction>().selectAttack("megaCata");
                            
                            if(chatBlancAction.firstLock){
                                dialogueBox.GetComponent<Dialogue>().restartDialoge("MegaCataSecondTime");
                            }
                            else {
                                dialogueBox.GetComponent<Dialogue>().restartDialoge("MegaCataCast");
                            }
                            chatBlancAction.megaCataActive = true;
                        }

                    }   
                
            }    
                

            }

                howManyPlayed++;
                turn = howManyPlayed/numberOfCharactersInOneTurn;
        }
    }

    private int special(GameObject currentUnit, int timer, GameObject battleMenu, GameObject battleMenySpecial, string animationName)
    {
        var currentUnitFighterStats =  currentUnit.GetComponent<FighterStats>();
        bool isSpecialActivated =currentUnitFighterStats.isSpecialActivated;
        if (isSpecialActivated)
        {
            if (timer == 2)
            {
                if(currentUnit.name == "ChatNoir"){
                     currentUnitFighterStats.specialParticles.SetActive(false);
                }
                currentUnit.GetComponent<Animator>().Play(animationName);
                battleMenu.SetActive(false);
                battleMenySpecial.SetActive(false);
                timer = 0;

                currentUnitFighterStats.isSpecialActivated = false;
                currentUnitFighterStats.isSpecialUsed = false;
            }
            else{
                timer++;
            }
        }
       
        return timer;
    }



    private int reloading(FighterStats currentFighterStats, GameObject currentUnit, AnimatorClipInfo[] animation,
         int reloadTimer,string backwardAnimation, string idleAnimation, bool activateBattleMenu)
    {
        if(currentUnit.tag == "ChatNoir"){
            reloadTimer  = cnReloadTimer;
        }
        else{
            reloadTimer = lbReloadTimer;
        }
        if (reloadTimer == 2)
        {
            currentUnit.SetActive(true);
            reloadTimer = 0;
            currentUnit.GetComponent<Animator>().Play(backwardAnimation);
            
        }
        if (!currentUnit.activeSelf)
        {
            reloadTimer++;
            currentFighterStats.ContinueGame();
        }
        else
        {
            if (animation.Length > 0 && animation[0].clip.name.Contains("Block"))
            {
                currentUnit.GetComponent<Animator>().Play(idleAnimation);
                var currentUnitFighterStats = currentUnit.GetComponent<FighterStats>();
                 if(currentUnitFighterStats.isSpecialActivated && currentUnitFighterStats.specialParticles != null && !currentFighterStats.isSpecialUsed ){
                    currentUnitFighterStats.specialParticles.SetActive(true);
                }
            }
            this.battleMenuChatNoir.SetActive(activateBattleMenu);
            this.battleMenuSpecialChatNoir.SetActive(activateBattleMenu);

            this.battleMenuLadyBug.SetActive(!activateBattleMenu);
            this.battleMenuSpecialLadyBug.SetActive(!activateBattleMenu);


        }

        return reloadTimer;
    }

    private void calculateTauntTimer(GameObject chat){

        bool isTauntActivated = chat.GetComponent<FighterStats>().isTauntActivated;

        if(isTauntActivated){
            if(punTimer == 1){
                chat.GetComponent<FighterStats>().isTauntActivated = false;
                punTimer = 0;
            }
            else{
                 punTimer++;
            }
           
        }

    }

     private void calculateTeamworkTimer(GameObject ladyBug){
        bool isTeamWorkActivated = ladyBug.GetComponent<FighterStats>().isTeamWorkActivated;
        if(isTeamWorkActivated){
            if(teamWorkTimer == 1){

                ladyBug.GetComponent<FighterStats>().isTeamWorkActivated = false;
                if(chatNoir.activeSelf){
                    teamWorkButton.GetComponent<Button>().interactable = true;
                }
                teamWorkButton.transform.Find("TimerObject").gameObject.SetActive(false);
                teamWorkButton.GetComponent<Timer>().reset();
                teamWorkTimer = 3;

            }
            else{
                
                teamWorkTimer--;
                teamWorkButton.GetComponent<Timer>().decrease();
            }
           
        }
     }
}
