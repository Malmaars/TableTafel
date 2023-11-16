using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 gridSize;
    public Vector2 distance;
    public float rowOffset;
    public GameObject gridPart;
    // Start is called before the first frame update
    void Start()
    {
       //generate the Cubes
       for(float i = -gridSize.x / 2; i < gridSize.x / 2; i++)
        {
            for(float k = -gridSize.y / 2; k < gridSize.y / 2; k++)
            {
                if (k % 2 == 0)
                    Instantiate(gridPart, new Vector3(i * distance.x, 0, k * distance.y), new Quaternion(0, 0, 0, 0));

                else
                    Instantiate(gridPart, new Vector3(i * distance.x + rowOffset, 0, k * distance.y), new Quaternion(0, 0, 0, 0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
