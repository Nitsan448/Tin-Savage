using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer2D
{
    [RequireComponent(typeof(PlayerVelocityCalculator))]
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        // public CharacterController Controller => _controller;
        public event Action<float> HealthChanged;
        public float MaxHealth => _maxHealth;
        public float Health => _health;

        private PlayerVelocityCalculator _playerVelocityCalculator;
        private CharacterController _controller;
        private SpriteRenderer _spriteRenderer;

        private float _health;
        [SerializeField] private float _maxHealth = 100;


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _controller = GetComponent<CharacterController>();
            _playerVelocityCalculator = GetComponent<PlayerVelocityCalculator>();
            _health = _maxHealth;
        }

        private void Start()
        {
            _playerVelocityCalculator.Init(_controller);
        }

        private void Update()
        {
            _playerVelocityCalculator.UpdateVelocity();
            UpdateFacingDirection();
        }

        private void FixedUpdate()
        {
            _controller.Move(_playerVelocityCalculator.Velocity * Time.deltaTime);
        }


        private void UpdateFacingDirection()
        {
            float horizontalInput = InputManager.Instance.DirectionalInputRaw.x;
            if (horizontalInput != 0)
            {
                _spriteRenderer.flipX = horizontalInput < 0;
            }
        }

        public void TakeDamage(float damageTaken)
        {
            _health -= damageTaken;
            HealthChanged?.Invoke(_health);
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Game over");
        }
    }
}