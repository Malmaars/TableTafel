using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum tileType
{
    square,
    hexagon,
    octagon
}
public class GridGenerator : MonoBehaviour
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
        BlackBoard.Initialize();
        gridArray = new ITile[(int)gridSize.x, (int)gridSize.y];
       //generate the Cubes
       for(int i = 0; i < gridSize.x; i++)
        {
            for(int k = 0; k < gridSize.y; k++)
            {
                GameObject visualTile;

                float xPos, yPos;

                xPos = i * distance.x;

                if (i % 2 == 0)
                    yPos = k * distance.y;

                else
                    yPos = k * distance.y + heightOffset;

                visualTile = Instantiate(gridPart, new Vector3(xPos, 0, yPos), Quaternion.Euler(rotation));


                switch (type)
                {
                    case tileType.hexagon:
                        gridArray[(int)i, (int)k] = new Hexagon(new Vector2Int(i, k), visualTile, fiducialColor.wasteland);
                        BlackBoard.AssignTile(gridArray[(int)i, (int)k]);
                        if (debugMode)
                        {
                            Instantiate(debugVisual, visualTile.transform);
                            visualTile.GetComponentInChildren<TextMeshPro>().text = i + ", " + k;
                        }
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
