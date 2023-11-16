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
                case fiducialColor.blue:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[0];
                    break;
                case fiducialColor.red:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[1];
                    break;
                case fiducialColor.yellow:
                    other.gameObject.GetComponent<MeshRenderer>().material = materialColors[2];
                    break;
            }
        }
    }
}
