using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public bool HoldingKey => _holdingKey;

    private bool _holdingKey = true;
    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private GameObject _keyOnPlayer;

    [SerializeField] private Vector3 _keyDropDistanceFromPlayer;

    [SerializeField] private float _minimumTimeBetweenDropToPickup;
    private float _keyDropTime;

    public void DropKey()
    {
        _keyDropTime = Time.time;
        _holdingKey = false;
        _keyOnPlayer.SetActive(false);
        Vector3 keySpawnPoint = transform.position + _keyDropDistanceFromPlayer;
        Instantiate(_keyPrefab, keySpawnPoint, _keyPrefab.transform.rotation);
    }

    public void PickUpKey(Key key)
    {
        if (Time.time < _keyDropTime + _minimumTimeBetweenDropToPickup)
        {
            return;
        }

        _keyOnPlayer.SetActive(true);
        _holdingKey = true;
        Destroy(key.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 keySpawnPoint = transform.position + _keyDropDistanceFromPlayer;
        Gizmos.DrawSphere(keySpawnPoint, 0.3f);
    }
}