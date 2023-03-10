using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;

public class MultiLanguage : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        LocalizationManager.Read();
        if(PlayerPrefs.GetString("language") != ""){
            LocalizationManager.Language = PlayerPrefs.GetString("language");
        }
    }


}
