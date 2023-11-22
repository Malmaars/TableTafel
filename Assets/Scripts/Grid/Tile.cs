using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public void ChangePosition(Vector2 _offset);
    public Vector2 position { get; set; }
    public GameObject visual { get; set; }
    public fiducialColor myColor { get; set; }
}

public class Hexagon : ITile
{
    public Hexagon(Vector2 _position, GameObject _visual, fiducialColor _color)
    {
        position = _position;
        visual = _visual;
        myColor = _color;
    }
    public Vector2 position { get; set; }
    public GameObject visual { get; set; }
    public fiducialColor myColor { get; set; }

    public void ChangePosition(Vector2 _offset)
    {
        position += _offset;
        visual.transform.position += new Vector3(_offset.x, 0, _offset.y);
    }
}
