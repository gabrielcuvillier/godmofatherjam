using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySFXRandom : MonoBehaviour
{
    [SerializeField] private float minTimeReloadRandom = 0f;
    [SerializeField] private float maxTimeReloadRandom = 1f;
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;
    private float audioClipLength;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClipLength = audioClip.length;
        StartCoroutine(PlaySFXCoroutine(CalculateNewDelay()));
    }

    private IEnumerator PlaySFXCoroutine(float delay)
    {
        PlaySFX();
        yield return new WaitForSeconds(audioClipLength);
        yield return new WaitForSeconds(delay);

        StartCoroutine(PlaySFXCoroutine(CalculateNewDelay()));
    }

    private float CalculateNewDelay()
    {
        float delay = Random.Range(minTimeReloadRandom, maxTimeReloadRandom);
        return delay;
    }

    private void PlaySFX()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
