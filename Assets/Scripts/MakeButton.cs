using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour
{
    private GameObject player;

    private RectTransform rectTransform;
    private RectTransform rectTransformSepecial;

    private GameObject actionMenu;

    private GameObject actionMenuSpecial;

    public bool swap = false;
    public bool swapActivated = false;


    [SerializeField]
    private Sprite cata1;

    [SerializeField]
    private Sprite cata2;


    [SerializeField]
    private GameObject spellDesc;

    [SerializeField]
    private GameObject spellName;

    void Start()
    {
        
        string temp = gameObject.name;
       
        gameObject.GetComponent<Button>().onClick.AddListener(() => attachCallback(temp));
        player = GameObject.FindGameObjectWithTag("LadyBug");
        actionMenu = GameObject.Find("Action Menu Ladybug");
        actionMenuSpecial = GameObject.Find("Action Menu Specials Ladybug");
    }

    private void attachCallback(string btn)
    {


        AudioManager.instance.activateSoundAttack("button1");
        setCurrentPlayer();

        rectTransform = actionMenu.GetComponent<RectTransform>();
        rectTransformSepecial = actionMenuSpecial.GetComponent<RectTransform>();


        if(!btn.Contains("Arrow") && !btn.Contains("LuckyCharm")){
            disableMenu();
            hideSpellDesc();
        }


        if (btn.CompareTo("Attack1") == 0)
        {
            player.GetComponent<FighterAction>().selectAttack("attack");
        }
        else if (btn.CompareTo("Block1") == 0)
        {
            player.GetComponent<FighterAction>().selectAttack("block");
        }
        else if (btn.CompareTo("Reload") == 0)
        {
            player.GetComponent<FighterAction>().selectAttack("reload");
        }
        else if (btn.CompareTo("LuckyCharm") == 0)
        {
            var boss =  GameObject.FindGameObjectWithTag("Boss");
            var animation = boss.GetComponent<Animator>();
            if(animation.GetLayerWeight(3) == 1f || animation.GetLayerWeight(2) == 1f || animation.GetLayerWeight(1) == 1f &&   gameObject.GetComponent<Button>().interactable == true){
                
                AudioManager.instance.activateSound("luckySound");
                player.GetComponent<FighterAction>().selectAttack("luckyCharm");
                gameObject.GetComponent<Button>().interactable = false;
                disableMenu();
                hideSpellDesc();
                resetMenuPosition();

            }
        }
        else if (btn.CompareTo("Sneak") == 0){
            
            player.GetComponent<FighterAction>().selectAttack("sneak");
            resetMenuPosition();

        }

        else if (btn.CompareTo("TeamWork") == 0){
            player.GetComponent<FighterAction>().selectAttack("teamWork");
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.transform.Find("TimerObject").gameObject.SetActive(true);
            
            resetMenuPosition();

        }
        else if (btn.CompareTo("Cataclysm") == 0)
        {

            if(player.GetComponent<FighterStats>().isSpecialActivated){
                gameObject.GetComponent<Button>().interactable = false;
                gameObject.GetComponent<Image>().sprite = cata1;
            }
            else{
                gameObject.GetComponent<Image>().sprite = cata2;
            }
            player.GetComponent<FighterAction>().selectAttack("cataclysm");

            resetMenuPosition();
        }
        else if(btn.CompareTo("Mlady") == 0)
        {
            player.GetComponent<FighterAction>().selectAttack("mlady");
            resetMenuPosition();
        }
        else if(btn.CompareTo("Pun") == 0)
        {
            player.GetComponent<FighterAction>().selectAttack("pun");

            resetMenuPosition();
        }
        else if (btn.CompareTo("Arrow") == 0)
        {
            swapActivated = true;
            swap = true;
        }
        else if (btn.CompareTo("Arrow2") == 0)
        {
            resetMenuPosition();
        }
    }


    public void showSpellDesc(){
        spellDesc.SetActive(true);
        spellName.SetActive(true);
    }

    public void hideSpellDesc(){
        spellDesc.SetActive(false);
        spellName.SetActive(false);
    }

    private void resetMenuPosition()
    {
        swapActivated = true;
        swap = false;
    }

    private void setCurrentPlayer()
    {
        if (this.transform.parent.name.Contains("Ladybug"))
        {
            player = GameObject.FindGameObjectWithTag("LadyBug");
            actionMenu = GameObject.Find("Action Menu Ladybug");
            actionMenuSpecial = GameObject.Find("Action Menu Specials Ladybug");

        }
        else
        {
            player = GameObject.FindGameObjectWithTag("ChatNoir");
            actionMenu = GameObject.Find("Action Menu Chat Noir");
            actionMenuSpecial = GameObject.Find("Action Menu Specials Chat Noir");
        }
    }

    private void disableMenu(){
        actionMenu.SetActive(false);
        actionMenuSpecial.SetActive(false);
    }
    void Update()
    {    
        if(swapActivated){
            if(swap){
                if(rectTransformSepecial.position.y <= -4.48 ){
                    actionMenuSpecial.transform.Translate(Vector3.up * 15 * Time.deltaTime);
                    actionMenu.transform.Translate(Vector3.down * 15 * Time.deltaTime);
                
                }else{
                    swapActivated = !swapActivated;
                }
            }
            else{
                if(rectTransform.position.y <= -4.48 ){
                    actionMenuSpecial.transform.Translate(Vector3.down * 15 * Time.deltaTime);
                    actionMenu.transform.Translate(Vector3.up * 15 * Time.deltaTime);
                
                }else{
                    swapActivated = !swapActivated;
                }
            }

        }

    }


    public void luckyCharm(){


            

       
    }


}
