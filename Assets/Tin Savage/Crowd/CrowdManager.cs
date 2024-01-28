using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CrowdManager : ASingleton<CrowdManager>
{
    [SerializeField] private AudioSource[] _laughTracks;

    [FormerlySerializedAs("_crowdAnimations")] [SerializeField]
    private Animation[] _crowdLaughAnimations;

    [SerializeField] private Vector2 _pitchRandomizationRange;
    [SerializeField] private Vector2 _timeBetweenLaughs;

    [FormerlySerializedAs("_kingAnimation")] [SerializeField]
    private Animation _kingLaughAnimation;

    [SerializeField] private AudioSource _kingLaugh;

    private void Start()
    {
        PlayLaughsByScore(Random.Range(0, 2)).Forget();
    }

    [Button]
    public async UniTask PlayLaughsByScore(int score)
    {
        for (int i = 0; i < score; i++)
        {
            if (i == 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(_timeBetweenLaughs.x, _timeBetweenLaughs.y)) / 3);
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(_timeBetweenLaughs.x, _timeBetweenLaughs.y)));
            }

            if (i == 2)
            {
                PlayKingLaugh();
            }
            else
            {
                PlayRandomLaughTrack();
            }
        }
    }

    [Button]
    private void GetLaughTracks()
    {
        _laughTracks = GetComponents<AudioSource>();
    }

    [Button]
    private void PlayKingLaugh()
    {
        float pitch = Random.Range(0.97f, 1.03f);
        _kingLaugh.pitch = pitch;
        _kingLaugh.Play();
        _kingLaughAnimation.Play();
    }

    [Button]
    private void PlayRandomLaughTrack()
    {
        float pitch = Random.Range(_pitchRandomizationRange.x, _pitchRandomizationRange.y);
        int laughTrackIndex = Random.Range(0, _laughTracks.Length);
        _crowdLaughAnimations[laughTrackIndex].Play();
        _laughTracks[laughTrackIndex].pitch = pitch;
        _laughTracks[laughTrackIndex].Play();
    }
}