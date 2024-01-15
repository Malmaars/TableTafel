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
            //if (currentHexagonPosition.x != checkPosition.x || currentHexagonPosition.y != checkPosition.y)
            //{
            //    CompareCurrentTiles();
            //}

            //RotateSomething();
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
        //if(currentRotation != fiducialController.angle)
        //{
        //    CompareCurrentTiles();
        //}
        if (fiducialController.Speed > 0.05f)
        {
            return;
        }
        radius += fiducialController.RotationSpeed * Time.deltaTime * 1;
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
            //Debug.Log(radius);
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
        currentSelection.Clear();
    }

    void ChangeHexagon(ITile targetTile)
    {
        if (!currentSelection.Contains(targetTile))
        {
            currentSelection.Add(targetTile);
            StartCoroutine(AnimateTile(targetTile));
        }
        else
        {
            return;
        }

        bool spawnFoliage = true;
        switch (thisColor)
        {
            case fiducialColor.water:
                    EditTile(targetTile, fiducialColor.water, 0);
                break;
            case fiducialColor.sand:
                    EditTile(targetTile, fiducialColor.sand, 1);
                break;
            case fiducialColor.grass:
                    EditTile(targetTile, fiducialColor.grass, 2);
                break;
            case fiducialColor.snow:
                    EditTile(targetTile, fiducialColor.snow, 3);
                break;
            case fiducialColor.savannah:
                    EditTile(targetTile, fiducialColor.savannah, 9);
                break;
            case fiducialColor.bamboo:
                    EditTile(targetTile, fiducialColor.bamboo, 5);
                break;
            case fiducialColor.cherryBlossom:
                    EditTile(targetTile, fiducialColor.cherryBlossom, 12);
                break;
        }

        if (targetTile.foliage != null)
        {
            //remove the foliage, throw the gameobject in the object pool
            StartCoroutine(foliageGenerator.RemoveFoliage(targetTile.foliage));
            targetTile.foliage = null;
        }

        int randomint = Random.Range(0, 30);
        if (randomint == 0 && spawnFoliage)
        {
            //create new foliage
            targetTile.foliage = foliageGenerator.GenerateFoliage(thisColor, targetTile.visual.transform.position);
            currentFoliage++;
        }

    }

    void ResetHexagon(ITile targetTile)
    {
        EditTile(targetTile, fiducialColor.wasteland, 7);
        if (targetTile.foliage != null && targetTile.foliage.foliageColor == thisColor)
        {
            StartCoroutine(foliageGenerator.RemoveFoliage(targetTile.foliage));
            targetTile.foliage = null;
            currentFoliage--;
        }
    }


    void EditTile(ITile _toEdit, fiducialColor newColor, int spriteIndex)
    {
        BlackBoard.UnAssignTile(_toEdit);
        _toEdit.visual.GetComponent<SpriteRenderer>().sprite = colorSprites[spriteIndex];
        _toEdit.myColor = newColor;
        BlackBoard.AssignTile(_toEdit);
    }

    IEnumerator AnimateTile(ITile _tile)
    {
        while (_tile.beingAnimated)
        {
            //wait
            yield return null;
        }

        _tile.beingAnimated = true;
        Vector3 oldTransform = _tile.originalSize;

        Vector3 target = oldTransform * 2;
        _tile.visual.transform.position += new Vector3(0, 1, 0);
        while (_tile.visual.transform.localScale != target)
        {
            _tile.visual.transform.localScale = Vector3.Lerp(_tile.visual.transform.localScale, target, Time.deltaTime * 50);
            yield return null; // Pause and resume on the next frame
        }

        while (_tile.visual.transform.localScale != oldTransform)
        {
            _tile.visual.transform.localScale = Vector3.Lerp(_tile.visual.transform.localScale, oldTransform, Time.deltaTime * 20);
            yield return null; // Pause and resume on the next frame
        }
        _tile.visual.transform.position -= new Vector3(0, 1, 0);
        _tile.visual.transform.localScale = oldTransform;
        _tile.beingAnimated = false;
    }
}
