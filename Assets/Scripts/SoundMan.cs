using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMan : MonoBehaviour
{
    public List<AudioClip> music = new List<AudioClip>();

    private AudioSource source;

    public AudioSource Source
    {
        get
        {
            if (source == null)
                source = GetComponent<AudioSource>();
            return source;
        }
    }

    public float time;
    int i;

    public void Awake()
    {
        Source.clip = music[(i++) % music.Count];
        Source.Play();
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time >= Source.clip.length)
        {
            time = 0;
            Source.clip = music[(i++) % music.Count];
            Source.Play();
        }
    }
}
