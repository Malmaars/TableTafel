using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiducialParent : MonoBehaviour
{

    //Local fiducial controller, this is necessary to run this class. Make sure it's attached to the same gameobject.
    private FiducialController fiducialController;

    public GameObject toControl;

    public float positionMultiplier;
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
        //to make sure 0,0,0 is the center, we have to subtract half of the multiplier. In reactivision, 0,0 is the upper left corner and 1,1 is the lower right corner.
        Vector3 newPosition = new Vector3(-fiducialController.screenPosition.x * positionMultiplier + positionMultiplier / 2, toControl.transform.position.y, -fiducialController.screenPosition.y * positionMultiplier + positionMultiplier / 2);
        toControl.transform.position = newPosition;
        toControl.transform.rotation = Quaternion.Euler(0, -fiducialController.angleDegrees, 0);
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
