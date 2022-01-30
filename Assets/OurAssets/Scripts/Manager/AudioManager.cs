using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        GameMusic,
        AdsMusic,
        BossMusic,
        PopupOpen,
        PopupClose,
        PopupBuy,
        LifeRestored,
        Damaged,
        Explosion,
        EnemyDestroyed,
        PlayerBullet,
        EnemyBullet,
        BossBullet,
        BossTurbines

    }

    public enum AudioAction
    {
        Play,
        Next,
        UnPause,
        ToBackground,
        ToForeground,
        Mute,
        UnMute,
        Pause,
        Stop
    }

    [SerializeField] private GameObject audioManagerPF = null;
    [SerializeField] private PoolableAudio _poolableAudioPrefab = null;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip adsMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip popupOpen;
    [SerializeField] private AudioClip popupClose;
    [SerializeField] private AudioClip popupBuy;
    [SerializeField] private AudioClip lifeRestored;
    [SerializeField] private AudioClip damaged;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip enemyDestroyed;
    [SerializeField] private AudioClip playerBullet;
    [SerializeField] private AudioClip enemyBullet;
    [SerializeField] private AudioClip bossBullet;
    [SerializeField] private AudioClip bossTurbines;

    [Header("Volume")]
    
    [SerializeField] [Range(0,1)] private float backgroundVol;
    [SerializeField] [Range(0,1)] private float foregroundVol;
    [SerializeField] [Range(0,1)] private float effectsVol;

    private GameObject audioManagerGO = null;
    private AudioSource asGameMusic;
    private AudioSource asAdsMusic;
    private AudioSource asBossTurbines;
    private AudioSource asBossMusic;

    private ObjectPool<PoolableAudio> _sourcePool;
    
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
        _sourcePool = new ObjectPool<PoolableAudio>(_poolableAudioPrefab, 100);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManagerGO = Object.Instantiate(audioManagerPF,Vector3.zero,Quaternion.identity);

        asGameMusic = audioManagerGO.AddComponent<AudioSource>();
        asGameMusic.clip = gameMusic;
        asGameMusic.loop = true;
        asGameMusic.volume = foregroundVol;
        asGameMusic.playOnAwake = false;
        asGameMusic.Stop();

        asAdsMusic = audioManagerGO.AddComponent<AudioSource>();
        asAdsMusic.clip = adsMusic;
        asAdsMusic.loop = true;
        asAdsMusic.volume = foregroundVol;
        asAdsMusic.playOnAwake = false;
        asAdsMusic.Stop();

        asBossMusic = audioManagerGO.AddComponent<AudioSource>();
        asBossMusic.clip = bossMusic;
        asBossMusic.loop = true;
        asBossMusic.volume = foregroundVol;
        asBossMusic.playOnAwake = false;
        asBossMusic.Stop();

        asBossTurbines = audioManagerGO.AddComponent<AudioSource>();
        asBossTurbines.clip = bossTurbines;
        asBossTurbines.loop = true;
        asBossTurbines.volume = effectsVol;
        asBossTurbines.playOnAwake = false;
        asBossTurbines.Stop();

    }

    public void ManageAudio(AudioAction action, SoundType type)
    {
        AudioSource source = null;
        PoolableAudio audio = _sourcePool.RequestObject();
        Debug.Log("ManageAudio with action " + action + " and type " + type + ".");
        switch (type)
        {
            // Not from pull
            case SoundType.GameMusic:
                source = asGameMusic;
                break;
            case SoundType.AdsMusic:
                source = asAdsMusic;
                break;
            case SoundType.BossTurbines:
                source = asBossTurbines;
                break;
            case SoundType.BossMusic:
                source = asBossMusic;
                break;

            // From pull
            case SoundType.PopupOpen:
                audio.DeactivateInSeconds(popupOpen.length);
                source = audio.AudioSrc;

                source.clip = popupOpen;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.PopupClose:
                audio.DeactivateInSeconds(popupClose.length);
                source = audio.AudioSrc;

                source.clip = popupClose;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.PopupBuy:
                audio.DeactivateInSeconds(popupBuy.length);
                source = audio.AudioSrc;

                source.clip = popupBuy;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.LifeRestored:
                audio.DeactivateInSeconds(lifeRestored.length);
                source = audio.AudioSrc;

                source.clip = lifeRestored;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.Damaged:
                audio.DeactivateInSeconds(damaged.length);
                source = audio.AudioSrc;

                source.clip = damaged;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.Explosion:
                audio.DeactivateInSeconds(explosion.length);
                source = audio.AudioSrc;

                source.clip = explosion;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.EnemyDestroyed:
                audio.DeactivateInSeconds(enemyDestroyed.length);
                source = audio.AudioSrc;

                source.clip = enemyDestroyed;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.PlayerBullet:
                audio.DeactivateInSeconds(playerBullet.length);
                source = audio.AudioSrc;

                source.clip = playerBullet;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.EnemyBullet:
                audio.DeactivateInSeconds(enemyBullet.length);
                source = audio.AudioSrc;

                source.clip = enemyBullet;
                source.loop = false;
                source.volume = effectsVol;
                break;
            case SoundType.BossBullet:
                audio.DeactivateInSeconds(bossBullet.length);
                source = audio.AudioSrc;

                source.clip = bossBullet;
                source.loop = false;
                source.volume = effectsVol;
                break;
            default:
                Debug.LogError("Default case with action " + action + " and type " + type + ".");
                break;
            
        }

        if (type == SoundType.PopupOpen || type == SoundType.PopupClose || type == SoundType.PopupBuy 
            || type == SoundType.LifeRestored || type == SoundType.Damaged || type == SoundType.Explosion 
            || type == SoundType.PlayerBullet || type == SoundType.EnemyBullet || type == SoundType.BossBullet) // Sound Effects (Pull)
        {
            switch (action)
            {
                case AudioAction.Play:
                    source.Play();
                    break;
            }
        } else { // Music (Fixed)
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

}
