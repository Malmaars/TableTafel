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
    public float rowOffset;
    public GameObject gridPart;
    public tileType type;

    GridController gridController;
    // Start is called before the first frame update
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        ITile[,] gridArray = new ITile[(int)gridSize.x, (int)gridSize.y];
       //generate the Cubes
       for(float i = -gridSize.x / 2; i < gridSize.x / 2; i++)
        {
            for(float k = -gridSize.y / 2; k < gridSize.y / 2; k++)
            {
                GameObject visualTile;
                if (k % 2 == 0)
                    visualTile = Instantiate(gridPart, new Vector3(i * distance.x, 0, k * distance.y), new Quaternion(0, 0, 0, 0));

                else
                    visualTile = Instantiate(gridPart, new Vector3(i * distance.x + rowOffset, 0, k * distance.y), new Quaternion(0, 0, 0, 0));

                switch (type)
                {
                    case tileType.hexagon:
                        new Hexagon(new Vector2(i, k), visualTile);
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
