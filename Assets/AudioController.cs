using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AudioController : MonoBehaviour
{

    public AudioSource Audio;
    public VideoPlayer Video;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeChange (float value)
    {
        if(Audio != null)
            Audio.volume = value;

        if(Video != null)
            Video.SetDirectAudioVolume(0, value);
    }
}
