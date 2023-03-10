using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.SimpleLocalization;


    public class Dialogue : MonoBehaviour
    {

        public TextMeshProUGUI textComponent;
        public List<string> key;

        public float textSpeed;

        public GameObject mainPanel;
        private string currentText;

        private int index;


        public string dialoguePage = "Dialogue1";

        public bool megaCataCast = false;

        private List<String> dialogueList = new List<string>();
        public GameObject faces;

        
        [SerializeField]
        private GameObject endMenu;

        [SerializeField]
        private GameObject endText;


        private bool isDance = false;



        // Start is called before the first frame update
        void Start()
        {
            textComponent.text = string.Empty;
            startDialogue();
        }

        void startDialogue()
        {

            readCsv(dialoguePage);
            index = 0;
            StartCoroutine(TypeLine());
        }

        IEnumerator TypeLine(){ 

            if(key.ElementAt(index) == "d2LadyBug1.Start4"){
                AudioManager.instance.activateSound("danceSound");
                isDance = true;
            }
            faces.GetComponent<SetFace>().setNewFace(key.ElementAt(index));
            var tempText = LocalizationManager.Localize(key.ElementAt(index));
            currentText = tempText;
            bool blip = true;
            bool blip2 = true;
            foreach (char c in tempText)
            {
                if(blip && blip2){
                    if(key.ElementAt(index).Contains("Lady")){
                        AudioManager.instance.activateSoundAttack("blipFemale");
                        
                    }
                    else{
                        AudioManager.instance.activateSoundAttack("blipMale");
                    }
                    blip = false;
                    blip2 = false;
                }
                else{
                    if(blip){
                        blip2 = true;
                    }
                    blip = true;
                }
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

        }


        void nextLine(){
            if(index < key.Count -1){
                index++;
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
        {
            megaCataCheck();
            winScreen();
            gameObject.SetActive(false);
            mainPanel.SetActive(false);
            if(isDance){
                isDance = false;
                AudioManager.instance.stopDance();
            }
        }
    }

    private void megaCataCheck()
    {
        if (megaCataCast)
        {
            megaCataCast = false;
            var boss = GameObject.FindGameObjectWithTag("Boss");
            boss.GetComponent<ChatBlancAction>().selectAttack("megaCataLaunch");
            Invoke("loseScreen",3);
            
        }
    }

    private void winScreen(){


        if(dialoguePage == "end"){
            var boss =  GameObject.FindGameObjectWithTag("Boss");
            boss.GetComponent<FadeOut>().startFading();
            endMenu.gameObject.SetActive(true);
            endText.gameObject.GetComponent<TMP_Text>().text = LocalizationManager.Localize("MainMenu.Win");
            AudioManager.instance.activateSound("victorySound");
    
        }
        else if(dialoguePage == "deathLady" || dialoguePage == "deathChat"){
            loseScreen();
        }
    }



    private void loseScreen(){
        endMenu.gameObject.SetActive(true);
        endText.gameObject.GetComponent<TMP_Text>().text = LocalizationManager.Localize("MainMenu.Defeat");
        AudioManager.instance.activateSound("defeatSound");

    }

    public void oncClick(){

                if(textComponent.text == currentText){
                    nextLine();
                }
                else{
                    StopAllCoroutines();
                    textComponent.text = currentText;
                }
        }


    public void skipDialogue(){
            megaCataCheck();
            winScreen();
            gameObject.SetActive(false);
            mainPanel.SetActive(false);
            if(isDance){
                isDance = false;
                AudioManager.instance.stopDance();
            }
            AudioManager.instance.activateSoundAttack("text");
        }

        public void setDialogue(){
            var boss =  GameObject.FindGameObjectWithTag("Boss");
            var animation = boss.GetComponent<Animator>();
            
            if(animation.GetLayerWeight(3) == 1f){
                 restartDialoge("LuckyCharmMegaCata");
            }
            else if(animation.GetLayerWeight(2) == 1f){
                restartDialoge("LuckyCharmSuperCata");
            }
            else if(animation.GetLayerWeight(1) == 1f){
                restartDialoge("LuckyCharmDamageIdle");
            }
            else
            {
                restartDialoge("LuckyCharmIdle");
            }


        }

        public void restartDialoge(string dialoguePageName)
        {

            if(!dialogueList.Contains(dialoguePageName) || dialoguePageName == "LuckyCharmIdle" ){
                dialoguePage = dialoguePageName;
                gameObject.SetActive(true);
                mainPanel.SetActive(true);
                key.Clear();
                Start();
                dialogueList.Add(dialoguePageName);
            }


        }

        private void readCsv(string docName){
            using(var reader = new StreamReader( Application.streamingAssetsPath + @"\" + docName + ".csv"))
            {

                
                string headerLine = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // var line = reader.ReadLine();
                    var text = line.Replace("\r\n", "\n").Replace("\"\"", "[_quote_]");
                    var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                    foreach (Match match in matches)
                    {
                        text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[_comma_]").Replace("\n", "[_newline_]"));
                    }
                    text = text.Replace("。", "。 ").Replace("、", "、 ").Replace("：", "： ").Replace("！", "！ ").Replace("（", " （").Replace("）", "） ").Trim();

                    var lines = text.Split('\n').Where(i => i != "").ToList();
                    var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();
                    

                        
                    var columns = lines[0].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[_quote_]", "\"").Replace("[_comma_]", ",").Replace("[_newline_]", "\n")).ToList();
                    
                    key.Add(columns[0]);
                }
            }
        }




    }
