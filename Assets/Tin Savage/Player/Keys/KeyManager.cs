using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private Animator _keyAnimator;
    public bool HoldingKey => _keyOnPlayer.activeSelf;

    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private GameObject _keyOnPlayer;

    [SerializeField] private Vector3 _keyDropDistanceFromPlayer;

    [SerializeField] private float _minimumTimeBetweenDropToPickup;
    private float _keyDropTime;

    private void Awake()
    {
        _keyAnimator = _keyOnPlayer.GetComponent<Animator>();
    }

    private void Start()
    {
        if (GameConfiguration.Instance.PlayTutorial)
        {
            _keyOnPlayer.SetActive(false);
        }
        else
        {
            _keyOnPlayer.SetActive(true);
        }
    }

    public void DropKey()
    {
        _keyDropTime = Time.time;
        _keyOnPlayer.SetActive(false);
        Vector3 keySpawnPoint = transform.position + _keyDropDistanceFromPlayer;
        Instantiate(_keyPrefab, keySpawnPoint, _keyPrefab.transform.rotation, parent: SceneReferencer.Instance.KeysParent.transform);
    }

    public void PickUpKey(Key key)
    {
        if (Time.time < _keyDropTime + _minimumTimeBetweenDropToPickup)
        {
            return;
        }

        _keyOnPlayer.SetActive(true);
        AudioManager.Instance.Play("KeyPickup");
        Destroy(key.gameObject);
    }

    public void PlayKeyChargeAnimation()
    {
        _keyAnimator.SetTrigger("Charge");
    }
}