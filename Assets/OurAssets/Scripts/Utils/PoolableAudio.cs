using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolableAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource = null;
    public AudioSource AudioSrc { get => _audioSource; }

    public void DeactivateInSeconds(float seconds)
    {
        StartCoroutine(InnerCoroutine());

        IEnumerator InnerCoroutine()
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }
    }
}
