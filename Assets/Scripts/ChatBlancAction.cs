using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBlancAction : MonoBehaviour
{
    private GameObject boss;
    private GameObject ladyBug;
    private GameObject chatNoir;


    [SerializeField]
    private GameObject attackPrefab1;

    [SerializeField]
    private GameObject attackPrefab2;

    [SerializeField]
    public GameObject damagePrefab;

    [SerializeField]
    private GameObject damageIdlePrefab;

    [SerializeField]
    private GameObject superCataPrefab;

    [SerializeField]
    private GameObject megaCataPrefab;

    [SerializeField]
    private GameObject superCataLaunchPrefab;


    [SerializeField]
    private GameObject weakness1;


    [SerializeField]
    private GameObject weakness2;


    [SerializeField]
    private GameObject weakness3;




    public int megaCataTimer = 0;
    public bool superCataActive = false;
    public bool megaCataActive = false;

    public bool firstLock = false;

    private bool secondLock = false;

    private bool thirdLock = false;

    public bool turnSkip = false;

    public Image[] furryBar;



    private void Awake() {
        ladyBug =  GameObject.FindGameObjectWithTag("LadyBug");
        chatNoir =  GameObject.FindGameObjectWithTag("ChatNoir");
        boss =  GameObject.FindGameObjectWithTag("Boss");
    }


    public void selectAttack(string btn)
    {


       
        GameObject victim = boss;
        int target = Random.Range(0, 2);
        if(ladyBug.activeSelf && chatNoir.activeSelf){
            if(chatNoir.GetComponent<FighterStats>().isTauntActivated){
                victim = chatNoir;
            }
            else if(target == 0){
                victim = ladyBug;
            }
            else{
                victim = chatNoir;
            }
        }
        else if(ladyBug.activeSelf && !chatNoir.activeSelf){
            victim = ladyBug;
        }
        else if( !ladyBug.activeSelf && chatNoir.activeSelf){
                victim = chatNoir;
        } else{
            victim = null;
        }

        

        switch (btn)
        {
            case "attack":
                if(victim == chatNoir){
                    attackPrefab1.GetComponent<AttackScriptChatBlanc>().attackSpell(victim);
                }
                else if(victim == ladyBug){
                    attackPrefab2.GetComponent<AttackScriptChatBlanc>().attackSpell(victim);
                }

            break;

            case "superCata":
                superCataPrefab.GetComponent<AttackScriptChatBlanc>().superCata();
                this.GetComponent<FighterStats>().continueAfter1Sec();
            break;

            case "superCataLaunch":
                superCataLaunchPrefab.GetComponent<AttackScriptChatBlanc>().superCataLaunch(victim);
            break;

            case "megaCata":
                megaCataPrefab.GetComponent<AttackScriptChatBlanc>().megaCata();
                this.GetComponent<FighterStats>().continueAfter1Sec();
            break;

            case "megaCataLaunch":
                Invoke("megaCataLaunch", 3);
            break;
        
        }
    }


    public void megaCataLaunch(){
        megaCataPrefab.GetComponent<AttackScriptChatBlanc>().megaCataLaunch(ladyBug, chatNoir);
    }


    public void takeHits(GameObject hero){
        var animation = hero.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();


            if(animation[0].clip.name == "SneakLadyBug" && megaCataActive && !firstLock && gameController.luckyTimer != 0)
            {
                var bossFighterStats =  gameObject.GetComponent<FighterStats>();
                bossFighterStats.megaCataCharge.SetActive(false);
                megaCataActive = false;
                megaCataTimer = 0;
                firstLock = true;
                AudioManager.instance.activateSoundAttack("heavyPunch");
                weakness1.GetComponent<FadeOut>().startFading();
                gameObject.GetComponent<Animator>().SetLayerWeight(3,0);
                turnSkip = true;
                gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("padlock1");
            }
            else if(animation[0].clip.name == "ChataclysmeAttack"){
                var bossFighterStats =  gameObject.GetComponent<FighterStats>();
                if(superCataActive){
                    if(!secondLock){
                        weakness2.GetComponent<FadeOut>().startFading();
                        gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("padlock2");
                    }
                    bossFighterStats.superCataCharge.SetActive(false);
                    superCataActive = false;
                    secondLock = true;
                    gameObject.GetComponent<Animator>().SetLayerWeight(2,0);
                    
                    hero.GetComponent<FighterStats>().receiveDamage(2);
                    var heroFightAction = hero.GetComponent<FighterAction>();
                    heroFightAction.damagePrefab.GetComponent<AttackScript>().damageAnimation();
                }
                if(megaCataActive && firstLock){
                    bossFighterStats.megaCataCharge.SetActive(false);
                    megaCataActive = false;
                    megaCataTimer = 0;
                    gameObject.GetComponent<Animator>().SetLayerWeight(3,0);
                }
                turnSkip = true;
            }
            else if(animation[0].clip.name == "SneakLadyBug" && 
            gameObject.GetComponent<Animator>().GetLayerWeight(1) == 1f && 
            chatNoir.GetComponent<FighterStats>().isTauntActivated &&
            ladyBug.GetComponent<FighterStats>().isTeamWorkActivated &&
            gameController.luckyTimer != 0
            ){
                var bossFighterStats =  gameObject.GetComponent<FighterStats>();
                if(!thirdLock){
                    AudioManager.instance.activateSoundAttack("heavyPunch");
                    weakness3.GetComponent<FadeOut>().startFading();

                }
                if(!firstLock || !secondLock){
                    gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("padlock3");
                }
                bossFighterStats.superCataCharge.SetActive(false);
                superCataActive = false;
                gameObject.GetComponent<Animator>().SetLayerWeight(2,0);
                thirdLock = true;
                turnSkip = true;
            }


            if(firstLock && secondLock && thirdLock){
                gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("end");
            }
    }


    public void stackFurry(int bossLimit){

        furryBar[bossLimit].gameObject.SetActive(false);
        furryBar[bossLimit+1].gameObject.SetActive(true);
    }
}
