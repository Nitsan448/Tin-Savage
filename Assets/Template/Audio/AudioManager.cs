using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioManager : ASingleton<AudioManager>
{
    [SerializeField] private Sound[] _sounds;

    protected override void DoOnAwake()
    {
        foreach (Sound sound in _sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
            sound.Source.outputAudioMixerGroup = sound.Output;
        }

        Play("Theme", 2);
    }

    public void Play(string soundName, float fadeInTime = 0)
    {
        Sound sound = Array.Find(_sounds, sound => sound.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound" + soundName + "not found");
            return;
        }

        if (fadeInTime != 0)
        {
            sound.Source.Play();
            StartCoroutine(Fade(sound, 0, 1, fadeInTime));
        }
        else
        {
            sound.Source.Play();
        }
    }

    private static IEnumerator Fade(Sound sound, float startingVolume, float targetVolume, float fadeTime = 0)
    {
        sound.Source.volume = startingVolume;
        float currentTime = 0;
        while (currentTime <= fadeTime - 0.01f)
        {
            sound.Source.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        sound.Source.volume = targetVolume;
        sound.Volume = sound.Source.volume;
        if (targetVolume == 0)
        {
            sound.Source.Stop();
        }
    }

    public void Stop(string soundName, float fadeOutTime = 0)
    {
        Sound sound = Array.Find(_sounds, sound => sound.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound" + soundName + "not found");
            return;
        }

        if (fadeOutTime != 0)
        {
            StartCoroutine(Fade(sound, 1, 0, fadeOutTime));
        }
        else
        {
            sound.Source.Stop();
        }
    }
}