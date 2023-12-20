using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public fiducialColor[] targetColor;
    int currentTargetIndex;
    int currentTargetResidence;
    int residenceMax;
    ITile currentTarget;
    GameObject visual;

    Vector3 destination;

    bool waiting;
    float waitTime;
    float waitTimer;
    public float moveSpeed;

    public Animator myAnimator;

    //make a function to move to a target and chill there

    private void Start()
    {
        visual = this.gameObject;
        myAnimator.SetBool("Still", true);
    }

    public void SetDestination(ITile newTarget)
    {
        myAnimator.SetBool("Still", false);
        currentTarget = newTarget;
        destination = newTarget.visual.transform.position;
        Debug.Log(destination);
        Vector3 direction = new Vector3(destination.x, 0, destination.z) - new Vector3(visual.transform.position.x, 0, visual.transform.position.z);
        visual.transform.forward = direction.normalized;

    }
    private void Update()
    {
        if (destination != null)
        {
            fiducialColor colorToGoTo = targetColor[0];
            if (currentTargetResidence >= residenceMax)
            {
                int randomIndex = Random.Range(0, targetColor.Length);

                colorToGoTo = targetColor[randomIndex];

                if(CheckTileList(colorToGoTo))
                {
                    currentTargetIndex = randomIndex;
                    residenceMax = Random.Range(8, 16);
                    currentTargetResidence = 0;
                }
            }

            else
            {
                if (!CheckTileList(colorToGoTo))
                {
                    currentTargetIndex++;
                }
                colorToGoTo = targetColor[currentTargetIndex];
            }
            switch (colorToGoTo)
            {
                case fiducialColor.water:
                    if(BlackBoard.waterTiles.Count > 0)
                        WalkToDestination(BlackBoard.waterTiles);
                    break;
                case fiducialColor.grass:
                    if (BlackBoard.grassTiles.Count > 0)
                        WalkToDestination(BlackBoard.grassTiles);
                    break;
                case fiducialColor.sand:
                    if (BlackBoard.sandTiles.Count > 0)
                        WalkToDestination(BlackBoard.sandTiles);
                    break;
                case fiducialColor.snow:
                    if (BlackBoard.snowTiles.Count > 0)
                        WalkToDestination(BlackBoard.snowTiles);
                    break;
                case fiducialColor.bamboo:
                    if (BlackBoard.bambooTiles.Count > 0)
                        WalkToDestination(BlackBoard.bambooTiles);
                    break;
                case fiducialColor.savannah:
                    if (BlackBoard.savannahTiles.Count > 0)
                        WalkToDestination(BlackBoard.savannahTiles);
                    break;
                case fiducialColor.sandwater:
                    if (BlackBoard.sandWaterTiles.Count > 0)
                        WalkToDestination(BlackBoard.sandWaterTiles);
                    break;
                case fiducialColor.watergrass:
                    if (BlackBoard.grassWaterTiles.Count > 0)
                        WalkToDestination(BlackBoard.grassWaterTiles);
                    break;
                case fiducialColor.tundra:
                    if (BlackBoard.tundraTiles.Count > 0)
                        WalkToDestination(BlackBoard.tundraTiles);
                    break;
                case fiducialColor.wasteland:
                    if (BlackBoard.wastelandTiles.Count > 0)
                        WalkToDestination(BlackBoard.wastelandTiles);
                    break;
            }
        }
        else
        {
            myAnimator.SetBool("Still", true);
        }
    }

    bool CheckTileList(fiducialColor _color)
    {
        switch (_color)
        {
            case fiducialColor.water:
                if (BlackBoard.waterTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.grass:
                if (BlackBoard.grassTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.sand:
                if (BlackBoard.sandTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.snow:
                if (BlackBoard.snowTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.bamboo:
                if (BlackBoard.bambooTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.savannah:
                if (BlackBoard.savannahTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.sandwater:
                if (BlackBoard.sandWaterTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.watergrass:
                if (BlackBoard.grassWaterTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.tundra:
                if (BlackBoard.tundraTiles.Count == 0)
                    return false;
                break;
            case fiducialColor.wasteland:
                if (BlackBoard.wastelandTiles.Count == 0)
                    return false;
                break;
        }

        return true;
    }

    public void WalkToDestination(List<ITile> tileList)
    {
        if(currentTarget == null)
        {
            //set new destination
            if (tileList.Count > 0)
            {
                int randomTileNumber = Random.Range(0, tileList.Count);
                SetDestination(tileList[randomTileNumber]);
                waiting = false;
                waitTimer = 0;
            }
        }
        if (Vector3.Distance(visual.transform.position, destination) > 1f)
        {
            Vector3 direction = new Vector3(destination.x - visual.transform.position.x, 0, destination.z - visual.transform.position.z).normalized;
            visual.transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (!waiting)
            {
                myAnimator.SetBool("Still", true);
                waiting = true;
                waitTimer = 0;
                waitTime = Random.Range(1f, 2f);
            }

            else
            {
                if (waitTimer > waitTime && tileList.Count > 0)
                {
                    //set new destination
                    int randomTileNumber = Random.Range(0, tileList.Count);
                    SetDestination(tileList[randomTileNumber]);
                    currentTargetResidence++;
                    waiting = false;
                }

                waitTimer += Time.deltaTime;
            }
        }
    }
}
