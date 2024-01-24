using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1;
    [Range(0.1f, 3f)] public float Pitch = 1;
    public string Name;
    [HideInInspector] public AudioSource Source;
    public bool Loop;
    public AudioMixerGroup Output;
}