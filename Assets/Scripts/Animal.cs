using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //THIS IS NOT EFFICIENT AND WILL ONLY BE USED FOR THE PLAYTEST ON 25/11
    public HexagonAreaParent hexagonParent;

    public fiducialColor targetColor;
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
    }

    public void SetDestination(ITile newTarget)
    {
        myAnimator.SetBool("Still", false);
        currentTarget = newTarget;
        destination = newTarget.visual.transform.position;
        Vector3 direction = new Vector3(destination.x, 0, destination.z) - new Vector3(visual.transform.position.x, 0, visual.transform.position.z);
        visual.transform.forward = direction.normalized;

    }
    private void Update()
    {
        if (destination != null && hexagonParent.fiducialController.isVisible)
        {
            WalkToDestination();
        }
        else
        {
            myAnimator.SetBool("Still", true);
        }
    }

    public void WalkToDestination()
    {
        if(currentTarget == null || currentTarget.myColor != targetColor)
        {
            //set new destination
            ITile[] tileArray = new ITile[hexagonParent.currentSelection.Count];
            hexagonParent.currentSelection.CopyTo(tileArray);

            if (tileArray.Length != 0)
            {
                int randomTileNumber = Random.Range(0, tileArray.Length);
                SetDestination(tileArray[randomTileNumber]);
                waiting = false;
                waitTimer = 0;
            }
        }
        if (Vector3.Distance(visual.transform.position, destination) > 1f)
        {
            visual.transform.position = Vector3.Lerp(visual.transform.position, new Vector3(destination.x, visual.transform.position.y, destination.z), moveSpeed * Time.deltaTime);
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
                if (waitTimer > waitTime)
                {
                    //set new destination
                    ITile[] tileArray = new ITile[hexagonParent.currentSelection.Count];
                    hexagonParent.currentSelection.CopyTo(tileArray);

                    int randomTileNumber = Random.Range(0, tileArray.Length);
                    SetDestination(tileArray[randomTileNumber]);

                    waiting = false;
                }

                waitTimer += Time.deltaTime;
            }
        }
    }
}
