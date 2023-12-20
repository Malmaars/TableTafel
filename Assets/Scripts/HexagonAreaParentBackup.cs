using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAreaParentBackup : MonoBehaviour
{
    //Local fiducial controller, this is necessary to run this class. Make sure it's attached to the same gameobject.
    //MAKE THIS PRIVATE AFTER 25/11
    public FiducialController fiducialController;

    public GameObject toControl;

    public int range;
    float radius;

    public float positionXMultiplier;
    public float positionYMultiplier;


    public fiducialColor thisColor;
    public Material[] materialColors;
    public Sprite[] colorSprites;

    //A hashset to save all the tiles that are currently in the area of this fiducial

    //MAKE THIS PRIVATE AFTER 25/11
    public HashSet<ITile> currentSelection;
    HashSet<ITile> toRemove;
    Vector2 currentHexagonPosition;
    float currentRotation;

    //make things look a little prettier
    FoliageGenerator foliageGenerator;

    public float maximumFoliage;
    int currentFoliage;

    // Start is called before the first frame update
    void Awake()
    {
        foliageGenerator = FindObjectOfType<FoliageGenerator>();
        toRemove = new HashSet<ITile>();
        currentSelection = new HashSet<ITile>();
        radius = range / 2;
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
            LeaveArea();
            HideObject();
        }

        if (fiducialController.IsVisible && toControl.activeSelf)
        {
            ApplyTransform();

            Vector2 checkPosition = BlackBoard.gridController.ConvertWorldPositionToHexagonPosition(new Vector2(toControl.transform.position.x, toControl.transform.position.z));
            if (currentHexagonPosition.x != checkPosition.x || currentHexagonPosition.y != checkPosition.y)
            {
                CompareCurrentTiles();
            }

            RotateSomething();
            foreach(ITile item in CheckArea())
            {
                if (!currentSelection.Contains(item))
                    ChangeHexagon(item);
            }
        }
    }

    void ApplyTransform()
    {
        //set a specific float for the positionMultiplier per range it's in
        //xPos = 15.4 xNeg = 15  YPos =10.9 YNeg = 12.8

        if (fiducialController.ScreenPosition.x >= 0.5f)
        {
            positionXMultiplier = 37.73f;
        }
        else
        {
            positionXMultiplier = 33.7f;
        }

        if (fiducialController.ScreenPosition.y >= 0.5f)
        {
            positionYMultiplier = 25.5f;
        }
        else
        {
            positionYMultiplier = 25.5f;
        }


        //to make sure 0,0,0 is the center, we have to subtract half of the multiplier. In reactivision, 0,0 is the upper left corner and 1,1 is the lower right corner.

        //The input from reactivision is widescreen, so 0-1 in width has a much bigger difference than 0-1 in height. To combat this, I divide them by the screenwidth (which I think is 1920x1080)
        Vector3 newPosition = new Vector3(-fiducialController.screenPosition.x * positionXMultiplier + positionXMultiplier / 2, toControl.transform.position.y, -fiducialController.screenPosition.y * positionYMultiplier + positionYMultiplier / 2);
        toControl.transform.position = newPosition + BlackBoard.offset;
        toControl.transform.rotation = Quaternion.Euler(0, -fiducialController.angleDegrees, 0);
    }

    void RotateSomething()
    {
        if(currentRotation != fiducialController.angle)
        {
            CompareCurrentTiles();
        }
        if (fiducialController.Speed > 0.05f)
        {
            return;
        }
        radius += fiducialController.RotationSpeed * Time.deltaTime * 45;
        if (radius > range)
            radius = range;
        else if (radius < 0)
            radius = 0;

        currentRotation = fiducialController.angle;
        maximumFoliage = radius * 4;
    }

    void HideObject()
    {
        toControl.SetActive(false);
    }

    void ShowObject()
    {
        toControl.SetActive(true);
    }

    void CompareCurrentTiles()
    {
        foreach(ITile item in currentSelection)
        {
            if(!CheckArea().Contains(item))
            {
                ResetHexagon(item);
                toRemove.Add(item);
                continue;
            }
        }

        foreach(ITile item in toRemove)
        {
            currentSelection.Remove(item);
        }
        toRemove.Clear();
    }

    HashSet<ITile> CheckArea()
    {
        HashSet<ITile> newSelection = new HashSet<ITile>();

        //get the hexagon position of the object
        Vector2 positionOnGrid = BlackBoard.gridController.ConvertWorldPositionToHexagonPosition(new Vector2(toControl.transform.position.x, toControl.transform.position.z));
        int xGridPos = Mathf.RoundToInt(positionOnGrid.x);
        int yGridPos = Mathf.RoundToInt(positionOnGrid.y);

        currentHexagonPosition = new Vector2(xGridPos, yGridPos);

        for (int i = -Mathf.RoundToInt(radius); i <= Mathf.RoundToInt(radius); i++)
        {
            ITile targetTile = BlackBoard.gridController.Grid[xGridPos, yGridPos + i];
            newSelection.Add(targetTile);
        }
        int newYPos = yGridPos;
        float repeatingY = 1;

        float oppositeRepeatingY = 0;
        int oppositeYpos = yGridPos;
        if (xGridPos % 2 == 0)
        {
            repeatingY = 0;
            oppositeRepeatingY = 1;
        }

        int k = 0;
        int distance = 0;
        while (distance <= Mathf.RoundToInt(radius))
        {
            if (repeatingY == 2)
            {
                repeatingY = 0;
                newYPos++;
            }

            if (oppositeRepeatingY == 2)
            {
                oppositeRepeatingY = 0;
                oppositeYpos--;
            }

            ITile targetTile = BlackBoard.gridController.Grid[xGridPos + k, Mathf.RoundToInt(newYPos)];
            newSelection.Add(targetTile);

            targetTile = BlackBoard.gridController.Grid[xGridPos - k, Mathf.RoundToInt(newYPos)];
            newSelection.Add(targetTile);

            targetTile = BlackBoard.gridController.Grid[xGridPos - k, Mathf.RoundToInt(oppositeYpos)];
            newSelection.Add(targetTile);

            targetTile = BlackBoard.gridController.Grid[xGridPos + k, Mathf.RoundToInt(oppositeYpos)];
            newSelection.Add(targetTile);

            for (int p = 1; p < k; p++)
            {
                targetTile = BlackBoard.gridController.Grid[xGridPos + k, Mathf.RoundToInt(oppositeYpos) + p];
                newSelection.Add(targetTile);
                targetTile = BlackBoard.gridController.Grid[xGridPos - k, Mathf.RoundToInt(oppositeYpos) + p];
                newSelection.Add(targetTile);
            }


            for (int p = 1; p <= Mathf.RoundToInt(radius) - k; p++)
            {
                targetTile = BlackBoard.gridController.Grid[xGridPos + k, Mathf.RoundToInt(newYPos) + p];
                newSelection.Add(targetTile);
                targetTile = BlackBoard.gridController.Grid[xGridPos - k, Mathf.RoundToInt(newYPos) + p];
                newSelection.Add(targetTile);

                targetTile = BlackBoard.gridController.Grid[xGridPos + k, Mathf.RoundToInt(oppositeYpos) - p];
                newSelection.Add(targetTile);
                targetTile = BlackBoard.gridController.Grid[xGridPos - k, Mathf.RoundToInt(oppositeYpos) - p];
                newSelection.Add(targetTile);
            }

            oppositeRepeatingY++;
            repeatingY++;
            k++;
            distance++;
        }
        return newSelection;
    }

    void LeaveArea()
    {
        foreach (ITile item in currentSelection)
        {
            ResetHexagon(item);
        }
        currentSelection.Clear();
    }

    void ChangeHexagon(ITile targetTile)
    {
        if (!currentSelection.Contains(targetTile))
        {
            currentSelection.Add(targetTile);
        }
        else
        {
            return;
        }

        switch (thisColor)
        {
            case fiducialColor.water:
                if (targetTile.myColor == fiducialColor.sand)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[4];
                    targetTile.myColor = fiducialColor.tundra;
                }
                else if (targetTile.myColor == fiducialColor.snow)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[3];
                    targetTile.myColor = fiducialColor.grass;
                }
                else if (targetTile.myColor == fiducialColor.bamboo)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[6];
                    targetTile.myColor = fiducialColor.mixed;
                }
                else if (targetTile.myColor == fiducialColor.wasteland)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[0];
                    targetTile.myColor = fiducialColor.water;
                }
                break;
            case fiducialColor.sand:
                if (targetTile.myColor == fiducialColor.water)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[4];
                    targetTile.myColor = fiducialColor.tundra;
                }
                else if (targetTile.myColor == fiducialColor.snow)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[5];
                    targetTile.myColor = fiducialColor.bamboo;
                }
                else if (targetTile.myColor == fiducialColor.grass)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[6];
                    targetTile.myColor = fiducialColor.mixed;
                }
                else if (targetTile.myColor == fiducialColor.wasteland)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[1];
                    targetTile.myColor = fiducialColor.sand;
                }
                break;
            case fiducialColor.snow:
                if (targetTile.myColor == fiducialColor.water)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[3];
                    targetTile.myColor = fiducialColor.grass;
                }
                else if (targetTile.myColor == fiducialColor.sand)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[5];
                    targetTile.myColor = fiducialColor.bamboo;
                }
                else if (targetTile.myColor == fiducialColor.tundra)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[6];
                    targetTile.myColor = fiducialColor.mixed;
                }
                else if (targetTile.myColor == fiducialColor.wasteland)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[2];
                    targetTile.myColor = fiducialColor.snow;
                }
                break;
        }

        if (targetTile.foliage == null && currentFoliage < maximumFoliage)
        {
            int randomInt = Random.Range(0, 20);
            if (randomInt == 0)
            {
                //create new foliage
                targetTile.foliage = foliageGenerator.GenerateFoliage(thisColor, targetTile.visual.transform.position);
                currentFoliage++;
            }
        }
    }

    void ResetHexagon(ITile targetTile)
    {
        switch (thisColor)
        {
            case fiducialColor.water:
                if (targetTile.myColor == fiducialColor.water)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[7];
                    targetTile.myColor = fiducialColor.wasteland;
                }
                else if (targetTile.myColor == fiducialColor.tundra)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[1];
                    targetTile.myColor = fiducialColor.sand;
                }
                else if (targetTile.myColor == fiducialColor.grass)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[2];
                    targetTile.myColor = fiducialColor.snow;
                }
                else if (targetTile.myColor == fiducialColor.mixed)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[5];
                    targetTile.myColor = fiducialColor.bamboo;
                }
                break;
            case fiducialColor.sand:
                if (targetTile.myColor == fiducialColor.sand)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[7];
                    targetTile.myColor = fiducialColor.wasteland;
                }
                else if (targetTile.myColor == fiducialColor.bamboo)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[2];
                    targetTile.myColor = fiducialColor.snow;
                }
                else if (targetTile.myColor == fiducialColor.tundra)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[0];
                    targetTile.myColor = fiducialColor.water;
                }
                else if (targetTile.myColor == fiducialColor.mixed)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[3];
                    targetTile.myColor = fiducialColor.grass;
                }
                break;
            case fiducialColor.snow:
                if (targetTile.myColor == fiducialColor.snow)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[7];
                    targetTile.myColor = fiducialColor.wasteland;
                }
                else if (targetTile.myColor == fiducialColor.grass)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[0];
                    targetTile.myColor = fiducialColor.water;
                }
                else if (targetTile.myColor == fiducialColor.bamboo)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[1];
                    targetTile.myColor = fiducialColor.sand;
                }
                else if (targetTile.myColor == fiducialColor.mixed)
                {
                    targetTile.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[4];
                    targetTile.myColor = fiducialColor.tundra;
                }
                break;
        }

        if (targetTile.foliage != null && targetTile.foliage.foliageColor == thisColor)
        {
            foliageGenerator.RemoveFoliage(targetTile.foliage);
            targetTile.foliage = null;
            currentFoliage--;
        }
    }
}
