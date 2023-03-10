using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{

    public GameObject owner;
    Animator m_Animator;

    [SerializeField]
    private string animationName;

   
    private FighterStats attackerStats;
    private FighterStats targetStats;

     public void attackSpell(GameObject victim)
    {
       attackerStats = owner.GetComponent<FighterStats>();
       targetStats = victim.GetComponent<FighterStats>();
       owner.GetComponent<Animator>().Play(animationName);
       
            var animation = targetStats.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            if(animation[0].clip.name.Contains("Block")){
            }
            else{
            attackerStats.victim = victim;
            attackerStats.hit = true;
            }
    } 

    public void block(GameObject victim){
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();
        m_Animator = owner.GetComponent<Animator>();
        owner.GetComponent<Animator>().Play(animationName);
        AudioManager.instance.activateSoundAttack("spin");
        attackerStats.continueAfter1Sec();
    }


    public void reload(GameObject victim){
        owner.GetComponent<Animator>().Play(animationName);
    }


    public void luckyCharm(GameObject victim){
        owner.GetComponent<Animator>().Play(animationName);
    }

    public void sneak(GameObject victim){
        attackerStats = owner.GetComponent<FighterStats>();
        attackerStats.victim = victim;
        attackerStats.hit = true;
        
        owner.GetComponent<Animator>().Play(animationName);
    }
    public void teamWork(GameObject victim){
        owner.GetComponent<Animator>().Play(animationName);
    }

    public void cataclysme(GameObject victim){
        owner.GetComponent<Animator>().Play(animationName);
        if(animationName.Contains("Attack")){
            attackerStats = owner.GetComponent<FighterStats>();
            attackerStats.victim = victim;
            attackerStats.hit = true;
        }

        owner.GetComponent<FighterStats>().isSpecialActivated = true;
    }

    public void mlady(){
        owner.GetComponent<Animator>().Play(animationName);
    }

    public void pun(){
        AudioManager.instance.activateSoundAttack("taunt");
        owner.GetComponent<Animator>().Play(animationName);
    }


    public void damageAnimation(){
        if(owner.name == "ChatNoir"){
            owner.GetComponent<FighterStats>().specialParticles.SetActive(false);
            owner.GetComponent<Animator>().Play("ChatDamage");
        }
        else if(owner.name == "Ladybug"){
            owner.GetComponent<Animator>().Play("DamageLadyBug");
        }
    }



    public void dodge(){
         owner.GetComponent<Animator>().Play(animationName);
    }






}


