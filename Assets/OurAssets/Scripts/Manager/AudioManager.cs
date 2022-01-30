using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Game,
        Ads,
        Bullet,
        Explosion,
        Clic
    }

    public enum AudioAction
    {
        Play,
        UnPause,
        ToBackground,
        ToForeground,
        Mute,
        UnMute,
        Pause,
        Stop
    }

    [SerializeField] private GameObject audioManagerPF = null;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip adsMusic;
    [SerializeField] private AudioClip bulletSFX;
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private AudioClip clicSFX;

    [Header("Volume")]
    
    [SerializeField] [Range(0,1)] private float backgroundVol;
    [SerializeField] [Range(0,1)] private float foregroundVol;

    private GameObject audioManagerGO = null;
    private AudioSource asGameMusic;
    private AudioSource asAdsMusic;
    private AudioSource asBulletSFX;
    private AudioSource asExplosionSFX;
    private AudioSource asClicSFX;

    
    private static AudioManager _instance;
    public static AudioManager Instance { get => _instance; }

    private void Awake()
    {
        // Singleton implementation 
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // End of singleton implementation
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManagerGO = Object.Instantiate(audioManagerPF,Vector3.zero,Quaternion.identity);

        asGameMusic = audioManagerGO.AddComponent<AudioSource>();
        asGameMusic.clip = gameMusic;
        asGameMusic.loop = true;
        asGameMusic.Stop();

        asAdsMusic = audioManagerGO.AddComponent<AudioSource>();
        asAdsMusic.clip = adsMusic;
        asAdsMusic.loop = true;
        asAdsMusic.Stop();

        asBulletSFX = audioManagerGO.AddComponent<AudioSource>();
        asBulletSFX.clip = bulletSFX;
        asBulletSFX.loop = false;
        asBulletSFX.Stop();

        asExplosionSFX = audioManagerGO.AddComponent<AudioSource>();
        asExplosionSFX.clip = explosionSFX;
        asExplosionSFX.loop = false;
        asExplosionSFX.Stop();

        asClicSFX = audioManagerGO.AddComponent<AudioSource>();
        asClicSFX.clip = clicSFX;
        asClicSFX.loop = false;
        asClicSFX.Stop();

    }

    public void ManageAudio(AudioAction action, SoundType type)
    {
        AudioSource source = null;
        switch (type)
        {
            case SoundType.Game:
                source = asGameMusic;
                break;
            case SoundType.Ads:
                source = asAdsMusic;
                break;
            case SoundType.Bullet:
                source = asBulletSFX;
                break;
            case SoundType.Explosion:
                source = asExplosionSFX;
                break;
            case SoundType.Clic:
                source = asClicSFX;
                break;
            
        }

        switch (action)
        {
            case AudioAction.Play:
                source.Play();
                break;
            case AudioAction.UnPause:
                source.UnPause();
                break;
            case AudioAction.ToBackground:
                source.volume = backgroundVol;
                break;
            case AudioAction.ToForeground:
                source.volume = foregroundVol;
                break;
            case AudioAction.Mute:
                source.mute = true;
                break;
            case AudioAction.UnMute:
                source.mute = false;
                break;
            case AudioAction.Pause:
                source.Pause();
                break;
            case AudioAction.Stop:
                source.Stop();
                break;
            
        }
    }

}
