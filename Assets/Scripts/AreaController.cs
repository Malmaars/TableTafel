using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : FidController
{
    fiducialColor thisColor;

    //0 = blue, 1 = red, 2 = yellow, 3 = green, 4 = purple, 5 = orange, 6 = black
    Material[] materialColors;

    public Material White;

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
                case fiducialColor.blue:
                    if (cubeColor == fiducialColor.red)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.purple;
                    }
                    else if (cubeColor == fiducialColor.yellow)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.green;
                    }
                    else if (cubeColor == fiducialColor.orange)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.black;
                    }
                    else if (cubeColor == fiducialColor.white)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.blue;

                    }
                    break;
                case fiducialColor.red:
                    if (cubeColor == fiducialColor.blue)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.purple;
                    }
                    else if (cubeColor == fiducialColor.yellow)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.orange;
                    }
                    else if (cubeColor == fiducialColor.green)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.black;
                    }
                    else if (cubeColor == fiducialColor.white)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.red;

                    }
                    break;
                case fiducialColor.yellow:
                    if (cubeColor == fiducialColor.blue)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.green;
                    }
                    else if (cubeColor == fiducialColor.red)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.orange;
                    }
                    else if (cubeColor == fiducialColor.purple)
                    {
                        mr.material = materialColors[6];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.black;
                    }
                    else if (cubeColor == fiducialColor.white)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.yellow;

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
                case fiducialColor.blue:
                    Debug.Log("Blue");
                    if (cubeColor == fiducialColor.blue)
                    {
                        mr.material = White;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.white;
                    }
                    else if (cubeColor == fiducialColor.purple)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.red;
                    }
                    else if (cubeColor == fiducialColor.green)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.yellow;
                    }
                    else if (cubeColor == fiducialColor.black)
                    {
                        mr.material = materialColors[5];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.orange;
                    }
                    break;
                case fiducialColor.red:
                    if (cubeColor == fiducialColor.red)
                    {
                        mr.material = White;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.white;
                    }
                    else if (cubeColor == fiducialColor.orange)
                    {
                        mr.material = materialColors[2];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.yellow;
                    }
                    else if (cubeColor == fiducialColor.purple)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.blue;
                    }
                    else if (cubeColor == fiducialColor.black)
                    {
                        mr.material = materialColors[3];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.green;
                    }
                    break;
                case fiducialColor.yellow:
                    if (cubeColor == fiducialColor.yellow)
                    {
                        mr.material = White;
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.white;
                    }
                    else if (cubeColor == fiducialColor.green)
                    {
                        mr.material = materialColors[0];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.blue;
                    }
                    else if (cubeColor == fiducialColor.orange)
                    {
                        mr.material = materialColors[1];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.red;
                    }
                    else if (cubeColor == fiducialColor.black)
                    {
                        mr.material = materialColors[4];
                        other.gameObject.GetComponent<CubeData>().thisColor = fiducialColor.purple;
                    }
                    break;
            }
        }
    }
}
