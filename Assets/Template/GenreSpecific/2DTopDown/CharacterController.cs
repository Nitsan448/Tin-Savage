using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown2D
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] [Range(0f, 100f)] private float _maxSpeed = 10f;
        [SerializeField] [Range(0f, 100f)] private float _maxAcceleration = 10f;

        private Vector2 _velocity;
        private Vector2 _desiredVelocity;
        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            Vector2 directionalInput = Vector2.ClampMagnitude(InputManager.Instance.DirectionalInputRaw, 1);
            _desiredVelocity = directionalInput * _maxSpeed;
        }

        private void FixedUpdate()
        {
            _velocity = _rigidBody.velocity;
            float maxSpeedChange = _maxAcceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, maxSpeedChange);
            _rigidBody.velocity = _velocity;
        }
    }
}