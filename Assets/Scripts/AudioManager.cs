using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
[SerializeField] AudioSource soundSource;

[SerializeField] AudioSource danceSource;

[SerializeField] AudioSource sfxSource;

[SerializeField] AudioClip battleSound;

[SerializeField] AudioClip menuSound;

[SerializeField] AudioClip finalPhase;
[SerializeField] AudioClip cataSound;
[SerializeField] AudioClip luckySound;
[SerializeField] AudioClip victorySound;
[SerializeField] AudioClip fullVictory;
[SerializeField] AudioClip defeatSound;

[SerializeField] AudioClip danceSound;
[SerializeField] AudioMixer mixer;

[SerializeField] List<AudioClip> chatNoirStick;
[SerializeField] AudioClip chatNoirAttack;

[SerializeField] List<AudioClip> ladyBugAttack;

[SerializeField] List<AudioClip> ladyBuSneakStart;

[SerializeField] List<AudioClip> ladyBuSneakHit;

[SerializeField] AudioClip purr;

[SerializeField] AudioClip cataPunchLoad;
[SerializeField] List<AudioClip> heavyPunch;
[SerializeField] List<AudioClip> cartoonPunch;

[SerializeField] List<AudioClip> block;

[SerializeField] List<AudioClip> spin;

[SerializeField] List<AudioClip> megaCataCharge;

[SerializeField] List<AudioClip> megaCataLaunch;


[SerializeField] AudioClip superCataLaunch;
[SerializeField] List<AudioClip> superCataExplosion;

[SerializeField] List<AudioClip> reloadPart1;

[SerializeField] AudioClip reloadPart2;

[SerializeField] AudioClip button1;
[SerializeField] AudioClip button2;
[SerializeField] AudioClip text;
[SerializeField] AudioClip blipMale;
[SerializeField] AudioClip blipFemale;


[SerializeField] AudioClip taunt;
public static AudioManager instance;

public const string MUSIC_KEY = "musicVolume";
public const string SFX_KEY = "SfxVolume";


 void Awake()
{
    if(instance == null){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }   
    else{
        Destroy(gameObject);
    }
}

 private void Start() {
    loadVolume();    
}



    public void stopMusic(){
        soundSource.Stop();
    }

    public void play(){
        soundSource.Play();
    }


    public void pause(){
        soundSource.Pause();
    }

    public void stopDance(){
        danceSource.Stop();
        play();
    }


    public void stopDanceInPauseMenu(){
        danceSource.Stop();
    }
    public void activateSound(string clipName){
        switch (clipName)
        {
            case "finalPhase":  
                soundSource.clip = finalPhase;
                soundSource.Play();
            break;

            case "cataSound":
                soundSource.PlayOneShot(cataSound);
            break;

            case "luckySound":
                soundSource.PlayOneShot(luckySound);
            break;

            case "victorySound":
                soundSource.clip = fullVictory;
                soundSource.Play();
            break;

            case "defeatSound":
                soundSource.clip = defeatSound;
                soundSource.loop = false;
                soundSource.Play();
            break;
            
            case "battleSound":
                soundSource.clip = battleSound;
                soundSource.Play();
            break;

            case "menuSound":
                soundSource.clip = menuSound;
                soundSource.Play();
            break;

            case "danceSound":
                danceSource.clip = danceSound;
                danceSource.Play();
                pause();
            break;
        }


        
    }

    public void activateSoundAttack(string clipName){

        switch (clipName)
        {

            case "ChatNoirReload":
                sfxSource.PlayOneShot(chatNoirStick[Random.Range(0,chatNoirStick.Count)]);
            break;
            case "ChatNoir":
                sfxSource.PlayOneShot(chatNoirAttack);
            break;
            case "Boss":
                sfxSource.PlayOneShot(cartoonPunch[Random.Range(0,cartoonPunch.Count)]);
            break;
            case "Ladybug":
                sfxSource.PlayOneShot(ladyBugAttack[Random.Range(0,ladyBugAttack.Count)]);
            break;

            case "SneakStart":
                 sfxSource.PlayOneShot(ladyBuSneakStart[Random.Range(0,ladyBuSneakStart.Count)]);
            break;

            case "Sneakhit":
                 sfxSource.PlayOneShot(ladyBuSneakHit[Random.Range(0,ladyBuSneakHit.Count)]);
            break;

            case"purr":
                sfxSource.clip = purr;
                sfxSource.Play();
            break;

            case"teamwork":
                sfxSource.clip = victorySound;
                soundSource.volume = soundSource.volume / 2;
                Invoke("restoreSoundSourceVolume",sfxSource.clip.length - 1);
                sfxSource.Play();
            break;

            case"cataPunchLoad":
                sfxSource.PlayOneShot(cataPunchLoad);
            break;
            case"heavyPunch":
                sfxSource.clip = heavyPunch[Random.Range(0,heavyPunch.Count)];
                sfxSource.Play();
            break;
            case"cartoonPunch":
                sfxSource.PlayOneShot(cartoonPunch[Random.Range(0,cartoonPunch.Count)]);
            break;

            case"block":
                sfxSource.PlayOneShot(block[Random.Range(0,block.Count)]);
            break;

            case"spin":
                sfxSource.PlayOneShot(spin[Random.Range(0,spin.Count)]);
            break;
            case"megaCataCharge":
                sfxSource.PlayOneShot(megaCataCharge[Random.Range(0,megaCataCharge.Count)]);
            break;
            case"megaCataLaunch":
                sfxSource.PlayOneShot(megaCataLaunch[Random.Range(0,megaCataLaunch.Count)]);
            break;
            case"superCataLaunch":
                sfxSource.PlayOneShot(superCataLaunch);
            break;
            case"superCataExplosion":
                sfxSource.PlayOneShot(superCataExplosion[Random.Range(0,superCataExplosion.Count)]);
            break;
            case"reloadPart1":
                sfxSource.PlayOneShot(reloadPart1[Random.Range(0,reloadPart1.Count)]);
            break;
            case"reloadPart2":
                sfxSource.PlayOneShot(reloadPart2);
            break;
            case"taunt":
                sfxSource.clip = taunt;
                sfxSource.Play();
            break;
            case"button1":
                sfxSource.PlayOneShot(button1);
            break;
            case"button2":
                sfxSource.PlayOneShot(button1);
            break;
            case"text":
                sfxSource.PlayOneShot(text);
            break;
            case"blipMale":
                sfxSource.PlayOneShot(blipMale);
            break;
            case"blipFemale":
                sfxSource.PlayOneShot(blipFemale);
            break;

        }


    }

    public void stopSFX(){
        sfxSource.Stop();
    }

    public void restoreSoundSourceVolume(){
    soundSource.volume = soundSource.volume * 2;
    }


    private void loadVolume(){
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }




}
