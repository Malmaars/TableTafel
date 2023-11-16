using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public float cubeSize;
    public GameObject tryPrefab;
    // Start is called before the first frame update
    void Start()
    {
       //generate the Cubes
       for(float i = -6; i < 6; i+=0.2f)
        {
            for(float k = -6; k <6; k+=0.2f)
            {
                Instantiate(tryPrefab, new Vector3(i, 0, k), new Quaternion(0, 0, 0, 0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
