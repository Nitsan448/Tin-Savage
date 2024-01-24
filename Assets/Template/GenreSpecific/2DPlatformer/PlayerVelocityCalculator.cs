using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D
{
    public class PlayerVelocityCalculator : MonoBehaviour
    {
        [SerializeField] private float _maxJumpHeight = 4;
        [HideInInspector] public Vector2 Velocity;

        [SerializeField] private float _minJumpHeight;
        [SerializeField] private float _timeToJumpApex = .4f;
        [SerializeField] private float _accelerationTimeAirborne = .2f;
        [SerializeField] private float _accelerationTimeGrounded = .1f;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private int _maxJumps = 1;
        [SerializeField] private float _coyoteTime = 0.1f;

        private float _coyoteTimeLeft;
        private int _jumpsLeft;
        private float _gravity;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private float _velocityXSmoothing;
        private float _horizontalInput;

        private bool _wasOnGround;
        private CharacterController _characterController;

        public void Init(CharacterController characterController)
        {
            _characterController = characterController;
            _jumpsLeft = _maxJumps;
            _gravity = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
            _maxJumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
            _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * _minJumpHeight);
        }

        public void UpdateVelocity()
        {
            HandleJumping();
            Vector2 directionalInput = InputManager.Instance.DirectionalInputRaw;
            float targetVelocityX = directionalInput.x * _movementSpeed;
            Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref _velocityXSmoothing,
                (_characterController.Collisions.Below) ? _accelerationTimeGrounded : _accelerationTimeAirborne);

            Velocity.y += _gravity * Time.deltaTime;
        }

        private void HandleJumping()
        {
            ResetVelocityIfCollidingWithGroundOrCeiling();
            UpdateCoyoteTime();

            bool onGround = _characterController.Collisions.Below;
            UpdateJumpsLeft(out bool shouldAirJump);

            bool shouldJumpFromGround = InputManager.Instance.JumpInputDown &&
                                        (_characterController.Collisions.IsAlmostHittingGround || _coyoteTimeLeft > 0);

            bool shouldJump = shouldAirJump || shouldJumpFromGround;
            if (shouldJump)
            {
                Velocity.y = _maxJumpVelocity;
                _characterController.Collisions.Below = false;
            }

            _wasOnGround = onGround;

            if (!InputManager.Instance.JumpInputUp) return;
            if (Velocity.y > _minJumpVelocity)
            {
                Velocity.y = _minJumpVelocity;
            }
        }

        private void ResetVelocityIfCollidingWithGroundOrCeiling()
        {
            if (_characterController.Collisions.Above || _characterController.Collisions.Below)
            {
                Velocity.y = 0;
            }
        }

        private void UpdateCoyoteTime()
        {
            if (_characterController.Collisions.Below)
            {
                _coyoteTimeLeft = _coyoteTime;
            }
            else
            {
                _coyoteTimeLeft -= Time.deltaTime;
            }
        }

        private void UpdateJumpsLeft(out bool shouldAirJump)
        {
            bool onGround = _characterController.Collisions.Below;
            if (onGround)
            {
                _jumpsLeft = _maxJumps;
            }

            bool leftGround = _wasOnGround && !onGround;
            if (leftGround)
            {
                _jumpsLeft--;
            }

            shouldAirJump = InputManager.Instance.JumpInputDown &&
                            (!onGround && _jumpsLeft > 0);
            if (shouldAirJump)
            {
                _jumpsLeft--;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 rayStartPosition = new(transform.position.x, GetComponent<BoxCollider2D>().bounds.min.y);

            Vector2 minJumpDirection = Vector2.up * _minJumpHeight;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayStartPosition, minJumpDirection);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayStartPosition + minJumpDirection,
                Vector2.up * _maxJumpHeight - minJumpDirection);
        }
    }
}