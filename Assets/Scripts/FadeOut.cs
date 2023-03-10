using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    [SerializeField]
    private GameObject weakness;

    [SerializeField]
    private Sprite openLock;

    IEnumerator FadeOutMethod(){

            var rend =  weakness.GetComponent<Image>();
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                rend.color = new Color(1, 1, 1, i);
                yield return null;
            }
    }



    IEnumerator FadeOutMethodChat(){

            var rend =  weakness.GetComponent<SpriteRenderer>();
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                rend.color = new Color(1, 1, 1, i);
                yield return null;
            }
    }







    public void startFading(){
        if(weakness.GetComponent<Image>() != null){
            weakness.GetComponent<Image>().overrideSprite = openLock;
            StartCoroutine(FadeOutMethod());
        }
        else if(weakness.GetComponent<SpriteRenderer>() != null){
            weakness.GetComponent<SpriteRenderer>().sprite = openLock;
            StartCoroutine(FadeOutMethodChat());
        }
    }
}
