using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject boss;
    private GameObject ladyBug;
    private GameObject chatNoir;


    [SerializeField]
    private GameObject attackPrefab1;

    [SerializeField]
    private GameObject attackPrefab2;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private GameObject reloadPrefab;

    [SerializeField]
    private GameObject luckyCharmPrefab;

    [SerializeField]
    private GameObject sneakPrefabLady;

    [SerializeField]
    private GameObject teamworkPrefabLady;

    [SerializeField]
    private GameObject cataclysmePrefab;

    [SerializeField]
    private GameObject cataclysmePrefabAttack;

    [SerializeField]
    private GameObject mladyPrefab;

    [SerializeField]
    private GameObject punPrefab;

    [SerializeField]
    public GameObject damagePrefab;

    [SerializeField]
    public GameObject dodgePrefab;


    private bool isChatBlock = false;





    private void Awake() {
        ladyBug =  GameObject.FindGameObjectWithTag("LadyBug");
        chatNoir =  GameObject.FindGameObjectWithTag("ChatNoir");
        boss =  GameObject.FindGameObjectWithTag("Boss");
    }

    public void chatActive(){
        chatNoir.SetActive(true);

        if(isChatBlock){
            chatNoir.GetComponent<Animator>().Play("ChatBlock");
            isChatBlock = false;
        }
    }
    public void chatSfx(){
                AudioManager.instance.activateSound("cataSound");
    }
    public void selectAttack(string btn)
    {
        if(gameObject.name == "ChatNoir"){
            gameObject.GetComponent<FighterStats>().specialParticles.SetActive(false);
        }  
       
        GameObject victim = boss;
    

        if (btn.CompareTo("attack") == 0)
        {
            attackPrefab1.GetComponent<AttackScript>().attackSpell(victim);
        
        }
        else if (btn.CompareTo("block") == 0)
        {
            blockPrefab.GetComponent<AttackScript>().block(victim);
        }
        else if (btn.CompareTo("reload") == 0)
        {
            var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
            gameController.cataTimer = 0;
            gameController.luckyTimer = 0;
            gameObject.GetComponent<FighterStats>().isSpecialActivated = false;
            gameObject.GetComponent<FighterStats>().isSpecialUsed = false;
            reloadPrefab.GetComponent<AttackScript>().reload(victim);

        }
        else if (btn.CompareTo("luckyCharm") == 0 ){
            luckyCharmPrefab.GetComponent<AttackScript>().luckyCharm(victim);
        }
        else if (btn.CompareTo("sneak") == 0 ){
            sneakPrefabLady.GetComponent<AttackScript>().sneak(victim);
        }
        else if (btn.CompareTo("teamWork") == 0 ){

            var animation = chatNoir.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            if(animation[0].clip.name.Contains("Block")){
                isChatBlock = true;
            }
            chatNoir.SetActive(false);
            var fighterStats =  gameObject.GetComponent<FighterStats>();
            var fighterStatsChat =  chatNoir.GetComponent<FighterStats>();
            fighterStats.isTeamWorkActivated = true;
            fighterStats.dodge = 100;
            fighterStatsChat.dodge = 100;
            fighterStats.teamworkParticles.SetActive(true);
            GameObject.Find("TeamWorkEffect").SetActive(true);
            teamworkPrefabLady.GetComponent<AttackScript>().teamWork(victim);
        }
        else if(btn.CompareTo("cataclysm") == 0){
            if(gameObject.GetComponent<FighterStats>().isSpecialActivated){
                cataclysmePrefabAttack.GetComponent<AttackScript>().cataclysme(victim);
            }
            else{
                cataclysmePrefab.GetComponent<AttackScript>().cataclysme(victim);   
            }
        }
        else if(btn.CompareTo("mlady") == 0){
            var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
            var lbObject = gameController.fighterStats.Find(x => x.tag == "LadyBug");
            var lbIndex = gameController.fighterStats.FindIndex(x => x.tag == "LadyBug");
            gameController.fighterStats.RemoveAt(lbIndex);
            gameController.fighterStats.Insert(0,lbObject);  
            mladyPrefab.GetComponent<AttackScript>().mlady();
 
        }
         else if(btn.CompareTo("pun") == 0){

            var gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
            gameController.punTimer = 0;
            gameObject.GetComponent<FighterStats>().isTauntActivated = true;
            punPrefab.GetComponent<AttackScript>().pun();
         }

         else if(btn.CompareTo("dodge") == 0){
            dodgePrefab.GetComponent<AttackScript>().dodge();

        }

    }
}
