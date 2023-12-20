using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCube : MonoBehaviour
{
    fiducialColor thisColor;
    Material[] materialColors;
    private void Awake()
    {
        thisColor = transform.parent.GetComponent<FiducialParent>().thisColor;
        materialColors = transform.parent.GetComponent<FiducialParent>().materialColors;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "GridCube")
        {
            switch (thisColor)
            {
                case fiducialColor.water:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[0];
                    break;
                case fiducialColor.sand:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[1];
                    break;
                case fiducialColor.snow:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[2];
                    break;
            }
        }
    }
}
