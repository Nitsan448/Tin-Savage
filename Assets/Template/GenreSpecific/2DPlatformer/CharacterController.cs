using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer2D
{
    public class CharacterController : RayCastController
    {
        public CollisionInfo Collisions;

        [SerializeField] private float _maxClimbAngle = 80;
        [SerializeField] private float _maxDescendAngle = 75;

        public void Move(Vector2 moveAmount)
        {
            UpdateRaycastOrigins();
            Collisions.Reset();
            Collisions.VelocityOld = moveAmount;

            if (moveAmount.y < 0)
            {
                DescendSlope(ref moveAmount);
            }

            if (moveAmount.x != 0)
            {
                HorizontalCollisions(ref moveAmount);
            }

            if (moveAmount.y != 0)
            {
                VerticalCollisions(ref moveAmount);
            }

            Collisions.IsAlmostHittingGround = IsAlmostHittingGround(moveAmount);

            transform.Translate(moveAmount);
        }


        private void DescendSlope(ref Vector2 velocity)
        {
            RaycastHit2D hit = GetHitSlope(velocity);
            if (!hit) return;

            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            bool slopeAngleIsValid = slopeAngle != 0 && slopeAngle <= _maxDescendAngle;
            bool descendingSlope = hit.normal.x.Sign() == velocity.x.Sign();
            if (!slopeAngleIsValid || !descendingSlope) return;

            //TODO: Refactor
            if (!(hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))) return;

            float moveDistance = Mathf.Abs(velocity.x);
            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * velocity.x.Sign();
            velocity.y -= descendVelocityY;
            Collisions.SlopeAngle = slopeAngle;
            Collisions.DescendingSlope = true;
            Collisions.Below = true;
        }

        private RaycastHit2D GetHitSlope(Vector2 velocity)
        {
            Vector2 rayOrigin = velocity.x.Sign() == -1 ? _rayCastOrigins.BottomRight : _rayCastOrigins.BottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, GroundCollisionMask);
            return hit;
        }

        private void HorizontalCollisions(ref Vector2 velocity)
        {
            int directionX = velocity.x.Sign();
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = directionX == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.BottomRight;
                rayOrigin += Vector2.up * (_horizontalRaySpacing * i);

                RaycastHit2D groundHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, GroundCollisionMask);
                if (!groundHit) continue;

                float slopeAngle = Vector2.Angle(groundHit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= _maxClimbAngle)
                {
                    if (Collisions.DescendingSlope)
                    {
                        Collisions.DescendingSlope = false;
                        velocity = Collisions.VelocityOld;
                    }

                    float distanceToSlopeStart = 0;
                    if (slopeAngle != Collisions.SlopeAngleOld)
                    {
                        distanceToSlopeStart = groundHit.distance - SkinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (Collisions.ClimbingSlope && !(slopeAngle > _maxClimbAngle)) continue;

                velocity.x = Mathf.Min(Mathf.Abs(velocity.x), (groundHit.distance - SkinWidth) * directionX);
                rayLength = Mathf.Min(Mathf.Abs(velocity.x) + SkinWidth, groundHit.distance);

                if (Collisions.ClimbingSlope)
                {
                    velocity.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
                }

                Collisions.Left = directionX == -1;
                Collisions.Right = directionX == 1;
            }
        }

        private void ClimbSlope(ref Vector2 velocity, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            if (velocity.y > climbVelocityY) return;

            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * velocity.x.Sign();
            Collisions.Below = true;
            Collisions.ClimbingSlope = true;
            Collisions.SlopeAngle = slopeAngle;
        }

        private void VerticalCollisions(ref Vector2 velocity)
        {
            int directionY = velocity.y.Sign();
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            Vector2 rayOrigin;
            RaycastHit2D groundHit;
            for (int i = 0; i < VerticalRayCount; i++)
            {
                rayOrigin = directionY == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.TopLeft;
                rayOrigin += Vector2.right * (_verticalRaySpacing * i + velocity.x);
                groundHit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, GroundCollisionMask);

                if (!groundHit) continue;
                velocity.y = (groundHit.distance - SkinWidth) * directionY;
                rayLength = groundHit.distance;

                if (Collisions.ClimbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * velocity.x.Sign();
                }

                Collisions.Below = directionY == -1;
                Collisions.Above = directionY == 1;
            }

            if (!Collisions.ClimbingSlope) return;

            int directionX = velocity.x.Sign();
            rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            rayOrigin = ((directionX == -1) ? _rayCastOrigins.BottomLeft : _rayCastOrigins.BottomRight) +
                        Vector2.up * velocity.y;
            groundHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, GroundCollisionMask);

            if (!groundHit) return;
            float slopeAngle = Vector2.Angle(groundHit.normal, Vector2.up);

            if (slopeAngle == Collisions.SlopeAngle) return;
            velocity.x = (groundHit.distance - SkinWidth) * directionX;
            Collisions.SlopeAngle = slopeAngle;
        }

        private bool IsAlmostHittingGround(Vector2 velocity)
        {
            int directionY = velocity.y.Sign();

            if (directionY > 0 && velocity.y > 0.01f)
            {
                return false;
            }

            //TODO: serialize this ray length
            const float rayLength = 0.75f + SkinWidth;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = _rayCastOrigins.BottomLeft;
                rayOrigin += Vector2.right * (_verticalRaySpacing * i + velocity.x);
                RaycastHit2D groundHit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, GroundCollisionMask);

                if (groundHit) return true;
            }

            return false;
        }

        public struct CollisionInfo
        {
            public bool Above, Below;
            public bool Left, Right;

            public bool IsAlmostHittingGround;

            public bool ClimbingSlope;
            public float SlopeAngle, SlopeAngleOld;
            public bool DescendingSlope;
            public Vector2 VelocityOld;

            public void Reset()
            {
                Above = Below = false;
                Left = Right = false;
                ClimbingSlope = false;
                DescendingSlope = false;
                SlopeAngleOld = SlopeAngle;
                SlopeAngle = 0;
            }
        }
    }
}