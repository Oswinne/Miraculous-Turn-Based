using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{

    [SerializeField]
    private int limitY;

    [SerializeField]
    [Min(1)]
    private float defaultspeed = 2;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }


    void Update()
    {      
        if(rectTransform.position.y >= limitY){
             transform.Translate(-Vector3.up * defaultspeed * Time.deltaTime);
        }
    }



    
}
