using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{

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
}
