using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RayCastController : MonoBehaviour
    {
        public const float SkinWidth = 0.015f;
        [HideInInspector] public BoxCollider2D Collider;

        public LayerMask GroundCollisionMask;
        private const float distanceBetweenRays = 0.25f;
        protected int HorizontalRayCount;
        protected int VerticalRayCount;

        protected RaycastOrigins _rayCastOrigins;
        protected float _horizontalRaySpacing;
        protected float _verticalRaySpacing;


        private void Awake()
        {
            Collider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            CalculateRaySpacing();
        }

        protected void UpdateRaycastOrigins()
        {
            Bounds bounds = Collider.bounds;
            bounds.Expand(SkinWidth * -2);
            _rayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _rayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _rayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            _rayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = Collider.bounds;
            bounds.Expand(SkinWidth * -2);

            float boundsWidth = bounds.size.x;
            float boundsHeight = bounds.size.y;

            HorizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
            VerticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

            _horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
        }

        protected struct RaycastOrigins
        {
            public Vector2 TopLeft, TopRight;
            public Vector2 BottomLeft, BottomRight;
        }
    }
}