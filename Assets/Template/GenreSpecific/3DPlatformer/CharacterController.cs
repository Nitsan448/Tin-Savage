using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Platformer3D
{
    public class CharacterController : MonoBehaviour
    {
        public Rigidbody RigidBody => _rigidBody;
        public bool OnGround => _groundContactCount > 0;

        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 100f)] private float _maxGroundAcceleration = 10f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 10f;
        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 2f;
        [SerializeField, Range(0f, 1f)] private float _airJumpStrengthWhenFalling = 2;
        [SerializeField, Range(0f, 90f)] private float _maxGroundAngle = 25f;
        [SerializeField, Range(0f, 100f)] private float _maxSpeedToSnapToGround = 100f;
        [SerializeField, Min(0f)] private float _probeDistance = 1f;
        [SerializeField] private LayerMask _probeMask = -1;

        private Vector3 _velocity;
        private Vector3 _desiredVelocity;
        private Rigidbody _rigidBody;
        private bool _desiredJump;
        private float _minGroundDotProduct;
        private Vector3 _contactPointNormal;
        private int _groundContactCount;
        private int _physicsStepsSinceLastGrounded;
        private int _physicsStepsSinceLastJump;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            OnValidate();
        }

        private void OnValidate()
        {
            _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidBody.velocity = velocity;
            _velocity = velocity;
            _desiredVelocity = velocity;
        }

        public void UpdateInput()
        {
            Vector2 directionalInput = Vector2.ClampMagnitude(InputManager.Instance.DirectionalInput, 1);

            _desiredVelocity =
                new Vector3(directionalInput.x, 0f, directionalInput.y) * _maxSpeed;


            //TODO: add this to platformer controller
            //Once desired jump is set to true, it remains true until we set it to false
            _desiredJump = false;
        }


        public void CalculateVelocity()
        {
            UpdateState();
            AdjustVelocity();

            if (_desiredJump)
            {
                _desiredJump = false;
                Jump();
            }

            _rigidBody.velocity = _velocity;
            ClearState();
        }

        private void UpdateState()
        {
            _physicsStepsSinceLastGrounded++;
            _physicsStepsSinceLastJump++;
            _velocity = _rigidBody.velocity;
            if (OnGround || SnapToGround())
            {
                _physicsStepsSinceLastGrounded = 0;
                if (_groundContactCount > 1)
                {
                    _contactPointNormal.Normalize();
                }
            }
            else
            {
                _contactPointNormal = Vector3.up;
            }
        }

        private void AdjustVelocity()
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentXVelocity = Vector3.Dot(_velocity, xAxis);
            float currentZVelocity = Vector3.Dot(_velocity, zAxis);

            float acceleration = OnGround ? _maxGroundAcceleration : _maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

            float newXVelocity = Mathf.MoveTowards(currentXVelocity, _desiredVelocity.x, maxSpeedChange);
            float newZVelocity = Mathf.MoveTowards(currentZVelocity, _desiredVelocity.z, maxSpeedChange);

            _velocity += xAxis * (newXVelocity - currentXVelocity) + zAxis * (newZVelocity - currentZVelocity);
        }

        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - _contactPointNormal * Vector3.Dot(vector, _contactPointNormal);
        }

        private bool SnapToGround()
        {
            if (_physicsStepsSinceLastGrounded > 1 || _physicsStepsSinceLastJump <= 2)
            {
                return false;
            }

            float speed = _velocity.magnitude;
            if (speed > _maxSpeedToSnapToGround)
            {
                return false;
            }


            if (!Physics.Raycast(_rigidBody.position, Vector3.down, out RaycastHit hit, _probeDistance, _probeMask))
            {
                return false;
            }

            if (hit.normal.y < _minGroundDotProduct)
            {
                return false;
            }

            _groundContactCount = 1;
            _contactPointNormal = hit.normal;

            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0)
            {
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
            }

            return true;
        }

        private void Jump()
        {
            if (!OnGround) return;
            _physicsStepsSinceLastJump = 0;
            float jumpSpeed = CalculateJumpSpeed();
            _velocity.y += jumpSpeed;
        }

        private float CalculateJumpSpeed()
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _jumpHeight);
            if (_velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0);
            }
            else if (_velocity.y < 0)
            {
                //TODO: make this clearer
                jumpSpeed += -_velocity.y * _airJumpStrengthWhenFalling;
            }

            return jumpSpeed;
        }

        private void ClearState()
        {
            _groundContactCount = 0;
            _contactPointNormal = Vector3.zero;
        }

        private void OnCollisionEnter(Collision other)
        {
            EvaluateCollision(other);
        }

        private void OnCollisionStay(Collision other)
        {
            //Called after fixed update
            EvaluateCollision(other);
        }

        private void EvaluateCollision(Collision other)
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector3 normal = other.GetContact(i).normal;
                if (normal.y >= _minGroundDotProduct)
                {
                    _groundContactCount++;
                    _contactPointNormal += normal;
                }
            }
        }
    }
}