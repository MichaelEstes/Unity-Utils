using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Events/AudioEvent")]
public class AudioEvent : ScriptableObject
{
    public AudioClip[] clips;

    [MinMaxRange(0, 1)]
    public RangedFloat volume;

    [MinMaxRange(0, 2)]
    public RangedFloat pitch;

    public void Play(AudioSource source, bool oneShot = true)
    {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
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

    public void Play(AudioSource source, int index)
    {
        if (clips.Length == 0) return;

        source.clip = clips[index];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.Play();
    }

    public void Play(AudioSource source, AudioClip audioClip)
    {
        source.clip = audioClip;
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.Play();
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }
}
