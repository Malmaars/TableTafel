using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringGridGenerator : MonoBehaviour
{
    public Vector2 gridSize;
    public Vector2 distance;
    public float heightOffset;
    public Vector3 rotation;
    public GameObject gridPart;
    public tileType type;

    public bool debugMode;
    public GameObject debugVisual;

    public ITile[,] gridArray;
    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new ITile[(int)gridSize.x, (int)gridSize.y];
        //generate the Cubes
        for (float i = -(gridSize.x / 2); i < gridSize.x / 2; i++)
        {
            for (float k = -(gridSize.y / 2); k < gridSize.y / 2; k++)
            {
                GameObject visualTile;

                float xPos, yPos;

                xPos = i * distance.x;

                if (i % 2 == 0)
                    yPos = k * distance.y;

                else
                    yPos = k * distance.y + heightOffset;

                visualTile = Instantiate(gridPart, new Vector3(xPos, 0, yPos), Quaternion.Euler(rotation));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
