using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiducialParent : MonoBehaviour
{

    //Local fiducial controller, this is necessary to run this class. Make sure it's attached to the same gameobject.
    private FiducialController fiducialController;

    public GameObject toControl;

    // Start is called before the first frame update
    void Awake()
    {
        fiducialController = GetComponent<FiducialController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fiducialController.isVisible && !toControl.activeSelf)
        {
            ShowObject();
        }
        else if(!fiducialController.isVisible && toControl.activeSelf)
        {
            HideObject();
        }

        if (toControl.activeSelf)
            ApplyTransform();
    }

    void ApplyTransform()
    {
        toControl.transform.position = fiducialController.screenPosition;
        toControl.transform.rotation = Quaternion.Euler(0, 0, fiducialController.angleDegrees);
    }

    void HideObject()
    {
        toControl.SetActive(false);
    }

    void ShowObject()
    {
        toControl.SetActive(true);
    }
}
