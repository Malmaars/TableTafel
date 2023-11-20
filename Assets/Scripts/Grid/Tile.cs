using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public void ChangePosition(Vector2 _offset);
}

public class Hexagon : ITile
{
    public Hexagon(Vector2 _position, GameObject _visual)
    {
        position = _position;
        visual = _visual;
    }
    Vector2 position;
    GameObject visual;

    public void ChangePosition(Vector2 _offset)
    {
        position += _offset;
        visual.transform.position += new Vector3(_offset.x, 0, _offset.y);
    }
}
