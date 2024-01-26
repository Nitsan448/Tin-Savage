using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 4f;

    private void FixedUpdate()
    {
        Vector3 direction = FindObjectOfType<LookAtMouse>().transform.position - transform.position;
        direction.Normalize();
        transform.position += direction * _movementSpeed * Time.deltaTime;
    }
}