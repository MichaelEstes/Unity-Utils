using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCrossfader : MonoBehaviour
{
    public float transistionTime;


    private AudioSource l;
    private AudioSource r;

    void Awake()
    {
        l = gameObject.AddComponent<AudioSource>();
        r = gameObject.AddComponent<AudioSource>();
    }

    public void Play(AmbientAudioEvent audioEvent)
    {
        if (!l.isPlaying)
        {
            audioEvent.PlayLoop(l);
            return;
        }

        AudioSource temp = l;

        LeanTween.value(gameObject, v => { l.volume = v; }, l.volume, 0f, transistionTime).setOnComplete(() =>
        {
            temp.Stop();
        });

        audioEvent.PlayLoop(r);
        r.volume = 0;
        LeanTween.value(gameObject, v => { r.volume = v; }, 0f, audioEvent.volume.maxValue, transistionTime).setOnComplete(() =>
        {
            l = r;
            r = temp;
        });
    }
}
