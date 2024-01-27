using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaughterManager : ASingleton<LaughterManager>
{
    [SerializeField] private AudioSource[] _laughTracks;
    [SerializeField] private Vector2 _pitchRandomizationRange;
    [SerializeField] private Vector2 _timeBetweenLaughs;

    [Button]
    public async UniTask PlayLaughsByScore(int score)
    {
        for (int i = 0; i < score; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(_timeBetweenLaughs.x, _timeBetweenLaughs.y)));
            PlayRandomLaughTrack();
        }
    }

    [Button]
    private void GetLaughTracks()
    {
        _laughTracks = GetComponents<AudioSource>();
    }

    [Button]
    private void PlayRandomLaughTrack()
    {
        float pitch = Random.Range(_pitchRandomizationRange.x, _pitchRandomizationRange.y);
        int laughTrackIndex = Random.Range(0, _laughTracks.Length);
        _laughTracks[laughTrackIndex].pitch = pitch;
        _laughTracks[laughTrackIndex].Play();
    }
}