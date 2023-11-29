using System.Collections;
using UnityEngine;

public class AudioDelays : MonoBehaviour
{
    public AudioSource myAudio;

    [SerializeField]
    private float initialDelay = 2.0f; // Initial delay before the first play
    [SerializeField]
    private float loopDelay = 1.0f; // Delay between subsequent loops

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioWithDelay());
    }

    IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true) // Infinite loop for continuous playback
        {
            myAudio.Play();
            yield return new WaitForSeconds(myAudio.clip.length + loopDelay);
        }
    }
}
