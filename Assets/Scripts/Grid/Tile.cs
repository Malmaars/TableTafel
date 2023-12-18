using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public void ChangePosition(Vector2 _offset);

    //the position in the grid, not in world space
    public Vector2Int position { get; set; }
    public GameObject visual { get; set; }
    public fiducialColor myColor { get; set; }

    public foliage foliage { get; set; }

    public Vector3 originalSize { get; set; }
}

public class Hexagon : ITile
{
    public Hexagon( Vector2Int _position, GameObject _visual, fiducialColor _color)
    {

        position = _position;
        visual = _visual;
        myColor = _color;

        originalSize = _visual.transform.localScale;
    }

    public Vector2Int position { get; set; }
    public GameObject visual { get; set; }
    public fiducialColor myColor { get; set; }
    public foliage foliage { get; set; }

    public Vector3 originalSize { get; set; }

    public void ChangePosition(Vector2 _offset)
    {
        //position += _offset;
        visual.transform.position += new Vector3(_offset.x, 0, _offset.y);
    }
}
