using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFace : MonoBehaviour
{

public Image[] faces;


public void setNewFace(string name){
    foreach (var item in faces)
    {
        if(name.Contains(item.name)){
            item.gameObject.SetActive(true);
        }
        else{
            item.gameObject.SetActive(false);
        }
    }
}

}
