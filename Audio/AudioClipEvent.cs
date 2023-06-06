using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Events/AudioClipEvent")]
public class AudioClipEvent : ScriptableObject
{
    public AudioClip clip;

    [MinMaxRange(0, 1)]
    public RangedFloat volume;

    [MinMaxRange(0, 2)]
    public RangedFloat pitch;

    public void Play(AudioSource source, bool oneShot = true)
    {
        source.clip = clip;
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);

        if (oneShot)
        {
            source.PlayOneShot(source.clip, source.volume);
        }
        else
        {
            source.Play();
        }
    }

    public void Play(AudioSource source, AudioClip audioClip)
    {
        source.clip = audioClip;
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.Play();
    }

    public void PlayLoop(AudioSource source)
    {
        source.clip = clip;
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.loop = true;
        source.Play();
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }
}
