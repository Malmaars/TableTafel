using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//instead of tracking with colliders and such, I'll just track with code
public class GridController : MonoBehaviour
{
    //If I do it correcly, the index should be the same as their position
    public ITile[,] Grid;
    public GridGenerator gridGenerator;
    Vector2 wholeGridOffset = new Vector2(0,0);

    private void Awake()
    {
        BlackBoard.gridController = this;
        SetGridArray(gridGenerator.gridArray);
        MoveGrid(new Vector2(-20, -20));
    }

    //to avoid going through the entire array, we'll need to make custom grid positions
    public void SetGridArray(ITile[,] _newArray)
    {
        Grid = _newArray;
    }

    public Vector2 ConvertWorldPositionToHexagonPosition(Vector2 worldPosition)
    {
        worldPosition -= wholeGridOffset;
        //ok this one is a little more confusing, since I won't be able to know when not to remove the offsets.
        int hexagonX = Mathf.RoundToInt(worldPosition.x / gridGenerator.distance.x);
        int hexagonY;
        if (hexagonX % 2 == 0)
            hexagonY = Mathf.RoundToInt(worldPosition.y / gridGenerator.distance.y);
        else
            hexagonY = Mathf.RoundToInt((worldPosition.y - gridGenerator.heightOffset) / gridGenerator.distance.y);

        return new Vector2(hexagonX, hexagonY);
    }

    public Vector2 FindNearestHexagon()
    {
        return Vector2.zero;
    }
    
    public Vector2 ConvertHexagonPositionToWorldPosition(Vector2 hexagonPosition)
    {
        float xPos, yPos;
        xPos = hexagonPosition.x * gridGenerator.distance.x;

        if (hexagonPosition.y % 2 == 0)
            yPos = hexagonPosition.y * gridGenerator.distance.y;
        else
            yPos = hexagonPosition.y * gridGenerator.distance.y + gridGenerator.heightOffset;

        return new Vector2(xPos, yPos);
    }

    void MoveGrid(Vector2 newOffset)
    {
        wholeGridOffset += newOffset;
        BlackBoard.gridOffset = wholeGridOffset;

        for (int i = Grid.GetLength(0) - 1; i >= 0; i--)
        {
            for (int k = Grid.GetLength(1) - 1; k >= 0; k--)
            {
                Grid[i, k].ChangePosition(newOffset);
            }
        }
    }

    //ask for a tile in a specific direction. The direction will be an int from 1 to 6, instead of a vector2, since it differens if a hexagon is on even or odd position
    public ITile checkTileInWorldDirection(ITile _origin, int direction)
    {

        switch (direction)
        {
            case 1:
                return Grid[_origin.position.x, _origin.position.y + 1];
            case 2:
                if(_origin.position.x % 2 == 0)
                {

                }
                else
                {

                }

                break;
            case 3:
                break;
            case 4:
                return Grid[_origin.position.x, _origin.position.y - 1];
            case 5:
                break;
            case 6:
                break;
        }
        return null;
    }
}
