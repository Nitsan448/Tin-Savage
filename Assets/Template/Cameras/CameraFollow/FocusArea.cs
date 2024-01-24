using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FocusArea
{
    public Vector2 Center;
    public Vector2 Velocity;
    private Vector2 _size;

    public FocusArea(Transform target, Vector2 size)
    {
        Velocity = Vector2.zero;
        Center = target.position;
        _size = size;
    }

    public void Update(Transform target)
    {
        float shiftX = 0;
        if (target.position.x - _size.x / 2 > Center.x)
        {
            shiftX = target.position.x - _size.x / 2 - Center.x;
        }
        else if (target.position.x + _size.x / 2 < Center.x)
        {
            shiftX = target.position.x + _size.x / 2 - Center.x;
        }

        float shiftY = 0;
        if (target.position.y - _size.y / 2 > Center.y)
        {
            shiftY = target.position.y - _size.y / 2 - Center.y;
        }
        else if (target.position.y + _size.y / 2 < Center.y)
        {
            shiftY = target.position.y + _size.y / 2 - Center.y;
        }

        Center += new Vector2(shiftX, shiftY);
        Velocity = new Vector2(shiftX, shiftY);
    }
}