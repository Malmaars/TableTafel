using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiducialAreaParent : MonoBehaviour
{

    //Local fiducial controller, this is necessary to run this class. Make sure it's attached to the same gameobject.
    private FiducialController fiducialController;

    public GameObject toControl;

    public float positionXMultiplier;
    public float positionYMultiplier;

    public fiducialColor thisColor;
    public Material[] materialColors;
    // Start is called before the first frame update
    void Awake()
    {
        fiducialController = GetComponent<FiducialController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fiducialController.isVisible)
        {
            ShowObject();
        }
        else if(!fiducialController.isVisible)
        {
            HideObject();
        }

        if (toControl.activeSelf)
        {
            ApplyTransform();
            RotateSomething();
        }
    }

    void ApplyTransform()
    {
        //set a specific float for the positionMultiplier per range it's in
        //xPos = 15.4 xNeg = 15  YPos =10.9 YNeg = 12.8

        if(fiducialController.ScreenPosition.x >= 0.5f)
        {
            positionXMultiplier = 15.4f;
        }
        else
        {
            positionXMultiplier = 15f;
        }

        if(fiducialController.ScreenPosition.y >= 0.5f)
        {
            positionYMultiplier = 12.8f;
        }
        else
        {
            positionYMultiplier = 10.9f;
        }

        //to make sure 0,0,0 is the center, we have to subtract half of the multiplier. In reactivision, 0,0 is the upper left corner and 1,1 is the lower right corner.

        //The input from reactivision is widescreen, so 0-1 in width has a much bigger difference than 0-1 in height. To combat this, I divide them by the screenwidth (which I think is 1920x1080)
        Vector3 newPosition = new Vector3(-fiducialController.screenPosition.x * positionXMultiplier + positionXMultiplier / 2, toControl.transform.position.y, -fiducialController.screenPosition.y * positionYMultiplier + positionYMultiplier / 2);
        toControl.transform.position = newPosition + BlackBoard.offset;
        toControl.transform.rotation = Quaternion.Euler(0, -fiducialController.angleDegrees, 0);
    }

    void RotateSomething()
    {
        toControl.GetComponent<FidController>().RotateController(fiducialController.rotationSpeed, fiducialController.speed);
    }

    void HideObject()
    {
        if (toControl.GetComponent<AreaController>())
        {
            //code specifically for the area prototype
            toControl.GetComponent<MeshRenderer>().enabled = false;
            toControl.transform.position = new Vector3(toControl.transform.position.x, -10, toControl.transform.position.z);
        }
        else
        {
            toControl.SetActive(false);
        }
    }

    void ShowObject()
    {
        if (toControl.GetComponent<AreaController>())
        {
            toControl.GetComponent<MeshRenderer>().enabled = true;
            toControl.transform.position = new Vector3(toControl.transform.position.x, 0, toControl.transform.position.z);
        }
        else
        {
            toControl.SetActive(true);
        }
    }


}
