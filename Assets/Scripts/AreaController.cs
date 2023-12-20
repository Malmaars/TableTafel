using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : FidController
{
    fiducialColor thisColor;

    //0 = water, 1 = sand, 2 = snow, 3 = grass, 4 = tundra, 5 = bamboo, 6 = mixed
    Material[] materialColors;

    public Material wasteland;

    private void Awake()
    {
        thisColor = transform.parent.GetComponent<FiducialAreaParent>().thisColor;
        materialColors = transform.parent.GetComponent<FiducialAreaParent>().materialColors;
    }

    public override void RotateController(float rotSpeed, float moveSpeed)
    {
        if(moveSpeed > 0.05f)
        {
            return;
        }
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius += rotSpeed;
        if (sphereCollider.radius > 4)
            sphereCollider.radius = 4;
        else if (sphereCollider.radius < -4)
            sphereCollider.radius = -4;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GridCube")
        {
            MeshRenderer mr = other.gameObject.GetComponent<MeshRenderer>();
            fiducialColor cubeColor = other.gameObject.GetComponent<CubeData>().thisColor;
            switch (thisColor)
            {
                case fiducialColor.water:
                    if (cubeColor == fiducialColor.sand)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.tundra;
                    }
                    else if (cubeColor == fiducialColor.snow)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.grass;
                    }
                    else if (cubeColor == fiducialColor.bamboo)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.mixed;
                    }
                    else if (cubeColor == fiducialColor.wasteland)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.water;

                    }
                    break;
                case fiducialColor.sand:
                    if (cubeColor == fiducialColor.water)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.tundra;
                    }
                    else if (cubeColor == fiducialColor.snow)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.bamboo;
                    }
                    else if (cubeColor == fiducialColor.grass)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.mixed;
                    }
                    else if (cubeColor == fiducialColor.wasteland)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.sand;

                    }
                    break;
                case fiducialColor.snow:
                    if (cubeColor == fiducialColor.water)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.grass;
                    }
                    else if (cubeColor == fiducialColor.sand)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.bamboo;
                    }
                    else if (cubeColor == fiducialColor.tundra)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.mixed;
                    }
                    else if (cubeColor == fiducialColor.wasteland)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.snow;

                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GridCube")
        {
            MeshRenderer mr = other.gameObject.GetComponent<MeshRenderer>();
            fiducialColor cubeColor = other.gameObject.GetComponent<CubeData>().thisColor;
            switch (thisColor)
            {
                case fiducialColor.water:
                    Debug.Log("water");
                    if (cubeColor == fiducialColor.water)
                    {
                        mr.material = wasteland;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.wasteland;
                    }
                    else if (cubeColor == fiducialColor.tundra)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.sand;
                    }
                    else if (cubeColor == fiducialColor.grass)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.snow;
                    }
                    else if (cubeColor == fiducialColor.mixed)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.bamboo;
                    }
                    break;
                case fiducialColor.sand:
                    if (cubeColor == fiducialColor.sand)
                    {
                        mr.material = wasteland;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.wasteland;
                    }
                    else if (cubeColor == fiducialColor.bamboo)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.snow;
                    }
                    else if (cubeColor == fiducialColor.tundra)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.water;
                    }
                    else if (cubeColor == fiducialColor.mixed)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.grass;
                    }
                    break;
                case fiducialColor.snow:
                    if (cubeColor == fiducialColor.snow)
                    {
                        mr.material = wasteland;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.wasteland;
                    }
                    else if (cubeColor == fiducialColor.grass)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.water;
                    }
                    else if (cubeColor == fiducialColor.bamboo)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.sand;
                    }
                    else if (cubeColor == fiducialColor.mixed)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.tundra;
                    }
                    break;
            }
        }
    }
}
