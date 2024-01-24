using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCamera : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    [SerializeField] private float _minMaxX;

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime;
        //float verticalMovement = Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime;

        Vector3 newPosition = new Vector3(
            transform.position.x + horizontalMovement,
            transform.position.y,
            -10
        );

        if (newPosition.x < _minMaxX && newPosition.x > -_minMaxX)
        {
            transform.position = newPosition;
        }
    }
}