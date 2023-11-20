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
       for(float i = 0; i < gridSize.x; i++)
        {
            for(float k = 0; k < gridSize.y; k++)
            {
                GameObject visualTile;

                float xPos, yPos;

                xPos = i * distance.x;

                if (i % 2 == 0)
                    yPos = k * distance.y;

                else
                    yPos = k * distance.y + heightOffset;

                visualTile = Instantiate(gridPart, new Vector3(xPos, 0, yPos), new Quaternion(0, 0, 0, 0));


                switch (type)
                {
                    case tileType.hexagon:
                        gridArray[(int)i, (int)k] = new Hexagon(new Vector2(i, k), visualTile);
                        if (debugMode)
                            Instantiate(debugVisual, visualTile.transform);
                            visualTile.GetComponentInChildren<TextMeshPro>().text = i + ", " + k;
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
