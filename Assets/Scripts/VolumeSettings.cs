using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{

[SerializeField] AudioMixer mixer;
[SerializeField] Slider musicSlider;
[SerializeField] Slider sfxSlider;

public const string MIXER_MUSIC = "MusicVolume";
public const string MIXER_SFX = "SfxVolume";

    void Awake(){
        musicSlider.onValueChanged.AddListener(setMusicVolume);   
        sfxSlider.onValueChanged.AddListener(setSFXVolume);   
    }

    void Start() {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void setMusicVolume(float value){
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20); 
   }

       private void setSFXVolume(float value){
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20); 
   }

   private void OnDisable() {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
   }

    public void buttonSfx(){
        AudioManager.instance.activateSoundAttack("button2");
    }
}
