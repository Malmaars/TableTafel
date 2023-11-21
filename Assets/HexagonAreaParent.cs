using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAreaParent : MonoBehaviour
{
    //Local fiducial controller, this is necessary to run this class. Make sure it's attached to the same gameobject.
    private FiducialController fiducialController;

    public GameObject toControl;

    public int range;
    float radius;

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
        if (fiducialController.isVisible && toControl.activeSelf == false)
        {
            ShowObject();
        }
        else if (!fiducialController.isVisible && toControl.activeSelf == true)
        {
            HideObject();
        }

        if (toControl.activeSelf)
        {
            ApplyTransform();
            RotateSomething();
            CheckArea();
        }
    }

    void ApplyTransform()
    {
        //set a specific float for the positionMultiplier per range it's in
        //xPos = 15.4 xNeg = 15  YPos =10.9 YNeg = 12.8

        if (fiducialController.ScreenPosition.x >= 0.5f)
        {
            positionXMultiplier = 15.4f;
        }
        else
        {
            positionXMultiplier = 15f;
        }

        if (fiducialController.ScreenPosition.y >= 0.5f)
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
        if (fiducialController.RotationSpeed > 0.05f)
        {
            return;
        }
        radius += fiducialController.speed;
        if (radius > range)
            radius = range;
        else if (radius < -range)
            radius = -range;
    }

    void HideObject()
    {
        toControl.SetActive(false);
    }

    void ShowObject()
    {
        toControl.SetActive(true);
    }

    void CheckArea()
    {
        //get the hexagon position of the object
        Vector2 positionOnGrid = BlackBoard.gridController.ConvertWorldPositionToHexagonPosition(toControl.transform.position);
        int xGridPos = Mathf.RoundToInt(positionOnGrid.x);
        int yGridPos = Mathf.RoundToInt(positionOnGrid.y);

        //now get every hexagon in range
        for (int i = -Mathf.RoundToInt(radius); i <= Mathf.RoundToInt(radius); i++)
        {
            for (int k = -Mathf.RoundToInt(radius); k <= Mathf.RoundToInt(radius); k++)
            {
                if (i < 0 || k < 0 || i >= BlackBoard.gridController.Grid.Length || k >= BlackBoard.gridController.Grid.Length)
                    continue;
                ITile targetTile = BlackBoard.gridController.Grid[i, k];
                //targetTile.visual.GetComponent<MeshRenderer>().material = blue
                switch (thisColor)
                {
                    case fiducialColor.blue:
                        if (targetTile.myColor == fiducialColor.red)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[4];
                            targetTile.myColor = fiducialColor.purple;
                        }
                        else if (targetTile.myColor == fiducialColor.yellow)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[3];
                            targetTile.myColor = fiducialColor.green;
                        }
                        else if (targetTile.myColor == fiducialColor.orange)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[6];
                            targetTile.myColor = fiducialColor.black;
                        }
                        else if (targetTile.myColor == fiducialColor.white)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[0];
                            targetTile.myColor = fiducialColor.blue;

                        }
                        break;
                    case fiducialColor.red:
                        if (targetTile.myColor == fiducialColor.blue)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[4];
                            targetTile.myColor = fiducialColor.purple;
                        }
                        else if (targetTile.myColor == fiducialColor.yellow)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[5];
                            targetTile.myColor = fiducialColor.orange;
                        }
                        else if (targetTile.myColor == fiducialColor.green)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[6];
                            targetTile.myColor = fiducialColor.black;
                        }
                        else if (targetTile.myColor == fiducialColor.white)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[1];
                            targetTile.myColor = fiducialColor.red;

                        }
                        break;
                    case fiducialColor.yellow:
                        if (targetTile.myColor == fiducialColor.blue)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[3];
                            targetTile.myColor = fiducialColor.green;
                        }
                        else if (targetTile.myColor == fiducialColor.red)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[5];
                            targetTile.myColor = fiducialColor.orange;
                        }
                        else if (targetTile.myColor == fiducialColor.purple)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[6];
                            targetTile.myColor = fiducialColor.black;
                        }
                        else if (targetTile.myColor == fiducialColor.white)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[2];
                            targetTile.myColor = fiducialColor.yellow;

                        }
                        break;
                }
            }
        }
    }

    void LeaveArea()
    {
        //get the hexagon position of the object
        Vector2 positionOnGrid = BlackBoard.gridController.ConvertWorldPositionToHexagonPosition(toControl.transform.position);
        int xGridPos = Mathf.RoundToInt(positionOnGrid.x);
        int yGridPos = Mathf.RoundToInt(positionOnGrid.y);

        //now get every hexagon in range
        for (int i = -Mathf.RoundToInt(radius); i <= Mathf.RoundToInt(radius); i++)
        {
            for (int k = -Mathf.RoundToInt(radius); k <= Mathf.RoundToInt(radius); k++)
            {
                if (i < 0 || k < 0 || i >= BlackBoard.gridController.Grid.Length || k >= BlackBoard.gridController.Grid.Length)
                    continue;
                ITile targetTile = BlackBoard.gridController.Grid[i, k];

                switch (thisColor)
                {
                    case fiducialColor.blue:
                        Debug.Log("Blue");
                        if (targetTile.myColor == fiducialColor.blue)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[7];
                            targetTile.myColor = fiducialColor.white;
                        }
                        else if (targetTile.myColor == fiducialColor.purple)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[1];
                            targetTile.myColor = fiducialColor.red;
                        }
                        else if (targetTile.myColor == fiducialColor.green)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[2];
                            targetTile.myColor = fiducialColor.yellow;
                        }
                        else if (targetTile.myColor == fiducialColor.black)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[5];
                            targetTile.myColor = fiducialColor.orange;
                        }
                        break;
                    case fiducialColor.red:
                        if (targetTile.myColor == fiducialColor.red)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[7];
                            targetTile.myColor = fiducialColor.white;
                        }
                        else if (targetTile.myColor == fiducialColor.orange)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[2];
                            targetTile.myColor = fiducialColor.yellow;
                        }
                        else if (targetTile.myColor == fiducialColor.purple)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[0];
                            targetTile.myColor = fiducialColor.blue;
                        }
                        else if (targetTile.myColor == fiducialColor.black)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[3];
                            targetTile.myColor = fiducialColor.green;
                        }
                        break;
                    case fiducialColor.yellow:
                        if (targetTile.myColor == fiducialColor.yellow)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[7];
                            targetTile.myColor = fiducialColor.white;
                        }
                        else if (targetTile.myColor == fiducialColor.green)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[0];
                            targetTile.myColor = fiducialColor.blue;
                        }
                        else if (targetTile.myColor == fiducialColor.orange)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[1];
                            targetTile.myColor = fiducialColor.red;
                        }
                        else if (targetTile.myColor == fiducialColor.black)
                        {
                            targetTile.visual.GetComponent<MeshRenderer>().material = materialColors[4];
                            targetTile.myColor = fiducialColor.purple;
                        }
                        break;
                }
            }
        }
    }
}
