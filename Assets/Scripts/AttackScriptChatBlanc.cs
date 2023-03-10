using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScriptChatBlanc : MonoBehaviour
{

    public GameObject owner;
    Animator m_Animator;

    [SerializeField]
    private string animationName;

   
    private FighterStats attackerStats;

    private int bossLimit = 0;
    private float speed = 1;


    void Start(){
        attackerStats = owner.GetComponent<FighterStats>();
    }


    public void attackSpell(GameObject victim)
    {
        var targetStats = victim.GetComponent<FighterStats>();
        var animation = targetStats.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
       

        if(!animation[0].clip.name.Contains("Block")){
            attackerStats.hit = true;
            attackerStats.victim = victim;
        }
        else{
            attackerStats.hit = false;
        }

        owner.GetComponent<Animator>().Play(animationName);
        

    } 


    public void superCata(){
        
        attackerStats.superCataCharge.SetActive(true);
        owner.GetComponent<Animator>().SetLayerWeight(2,1);
        AudioManager.instance.activateSoundAttack("megaCataCharge");    
    }
        


    public void superCataLaunch(GameObject victim){
        attackerStats.victim = victim;
        attackerStats.hit = true;
        attackerStats.superCataCharge.SetActive(false);
        attackerStats.superCataLaunch.SetActive(true);
        owner.GetComponent<Animator>().Play(animationName);
        AudioManager.instance.activateSoundAttack("superCataLaunch");    

        if(victim.name == "ChatNoir"){
            attackerStats.superCataLaunch.GetComponent<Animator>().Play("superCataEffect");
        }
        else{
            attackerStats.superCataLaunch.GetComponent<Animator>().Play("SuperCataEffectLB");
        }
        Invoke("stopAnimation",1);

    }

    public void changeIdle(){
        owner.GetComponent<Animator>().SetLayerWeight(0,0);
    }

    public void megaCata(){


        AudioManager.instance.activateSoundAttack("megaCataCharge");         
        attackerStats.superCataCharge.SetActive(false);
        owner.GetComponent<Animator>().SetLayerWeight(2,0);
                   


        attackerStats.megaCataCharge.SetActive(true);
        owner.GetComponent<Animator>().SetLayerWeight(3,1);
    }

    public void megaCataLaunch(GameObject ladyBug, GameObject chatNoir)
    {

        AudioManager.instance.activateSoundAttack("megaCataLaunch");    
        attackerStats.megaCataCharge.GetComponent<Animator>().Play("MegaCataActiv");
        chatNoir.GetComponent<FighterStats>().specialParticles.SetActive(false);
        bool wasActivCN = chatNoir.activeSelf;
        bool wasActivLB = ladyBug.activeSelf;


        ladyBug.SetActive(true);
        chatNoir.SetActive(true);
        
        ladyBug.GetComponent<FighterStats>().receiveDamage(50);
        chatNoir.GetComponent<FighterStats>().receiveDamage(50);
        changeStatus(chatNoir,wasActivCN);
        changeStatus(ladyBug,wasActivLB);
        Invoke("stopCata", 1);
    }

    private static void changeStatus(GameObject hero, bool wasActive)
    {
        if (!wasActive)
        {
            hero.SetActive(false);
        }
    }

    public void damageAnimation(){
            if(! owner.GetComponent<ChatBlancAction>().superCataActive){
                 owner.GetComponent<Animator>().Play("DamageBoss");
            }
            if(bossLimit == 4){
                var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
                gameController.fighterStats.Add(owner.GetComponent<FighterStats>());
                owner.GetComponent<ChatBlancAction>().stackFurry(bossLimit);
                owner.GetComponent<Animator>().SetLayerWeight(1,1);
                gameController.numberOfCharactersInOneTurn++;
                bossLimit++;
                gameController.dialogueBox.GetComponent<Dialogue>().restartDialoge("DamageIdle");
                AudioManager.instance.activateSound("finalPhase");

            }
            else if(bossLimit < 4){
                owner.GetComponent<ChatBlancAction>().stackFurry(bossLimit);
                bossLimit++;
            }
            else{

                owner.GetComponent<Animator>().SetFloat("Speed", speed*=1.2f);
            }
        
    }


    private void stopCata(){
        attackerStats.megaCataCharge.SetActive(false);
        owner.GetComponent<Animator>().SetLayerWeight(3,0);
        attackerStats.ContinueGame();
    }


    private void stopAnimation(){
        if( attackerStats.superCataLaunch.activeSelf){
            owner.GetComponent<Animator>().SetLayerWeight(2,0);
            attackerStats.superCataLaunch.SetActive(false);
            attackerStats.AlertObservers("SuperCataLaunchEnded");
        }
    }
}
