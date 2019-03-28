using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // references this script possibly, not 100% sure about instances
    public static AudioManager instance;

    // number of audiotracks there are
    public int numberOfAudioTracks = 30;

    // array of all the "tracks" and audiosources
    public AudioSource[] tracks;

    // int responsible for keeping track of what specific track in the array we are playing on
    public int currentTrackNumber;

    public void Awake()
    {
        tracks = new AudioSource[numberOfAudioTracks];

        
        for (int i = 0; i < numberOfAudioTracks; i++)
        {
            var GO = new GameObject("Track" + (i + 1));
            GO.transform.parent = this.gameObject.transform;
            var AS = GO.AddComponent<AudioSource>();
            tracks[i] = AS;
        }


        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
