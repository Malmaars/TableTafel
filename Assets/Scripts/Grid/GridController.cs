using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//instead of tracking with colliders and such, I'll just track with code
public class GridController : MonoBehaviour
{
    //If I do it correcly, the index should be the same as their position
    ITile[,] Grid;

    //to avoid going through the entire array, we'll need to make custom grid positions
    public void SetGridArray(ITile[,] _newArray)
    {
        Grid = _newArray;
    }

    public void ConvertWorldPositionToHexagonPosition(Vector2 worldPosition)
    {

    }

    public void ConvertHexagonPositionToWorldPosition(Vector2 hexagonPosition)
    {

    }
}
