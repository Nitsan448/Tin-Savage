using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : Enemy
{
    [SerializeField] private List<GameObject> _spheres;
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    protected override void DoOnHit()
    {
        base.DoOnHit();
        GameObject sphereToDestroy = _spheres[_health];
        Destroy(sphereToDestroy);
        _collider.center = new Vector3(_collider.center.x,
            _collider.center.y + 1.1f, _collider.center.z);
    }
}