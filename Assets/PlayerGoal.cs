using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGoal : MonoBehaviour
{
    //This script is to give the players a goal, and reward them for completing that goal
    //the goal will consist of mutliple types of tiles, asking for specific amounts

    fiducialColor currentTargetColor = fiducialColor.wasteland;
    int currentTargetAmount;
    public Sprite[] TileSprites;

    public SpriteRenderer tileVisual;
    public Image goalMeter;

    bool completeAnimationBool = false;
    public ParticleSystem completeParticle;

    // Start is called before the first frame update
    void Start()
    {
        SetGoal(fiducialColor.water, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        if (!completeAnimationBool)
            CheckGoal();

        if (goalMeter.fillAmount > 1)
            goalMeter.fillAmount = 1;
    }

    void SetGoal(fiducialColor newTile, int targetAmount)
    {
        if(newTile == currentTargetColor)
        {
            Debug.LogWarning("Newtile shouldn't be the same as the completed tile mission");
            return;
        }

        //replace the sprite with the sprite of the new tile type
        switch (newTile)
        {
            case fiducialColor.water:
                if(targetAmount <= BlackBoard.waterTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[0];
                break;
            case fiducialColor.sand:
                if (targetAmount <= BlackBoard.sandTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[1];
                break;
            case fiducialColor.grass:
                if (targetAmount <= BlackBoard.grassTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[2];
                break;
            case fiducialColor.snow:
                if (targetAmount <= BlackBoard.snowTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[3];
                break;
            case fiducialColor.savannah:
                if (targetAmount <= BlackBoard.savannahTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[4];
                break;
            case fiducialColor.cherryBlossom:
                if (targetAmount <= BlackBoard.cherryBlossomTiles.Count)
                {
                    Debug.LogWarning("goal would be immediately met. Can't have that");
                    return;
                }
                tileVisual.sprite = TileSprites[5];
                break;
        }

        currentTargetColor = newTile;
        currentTargetAmount = targetAmount;
    }

    void CheckGoal()
    {
        float currentPercentage = 0;
        switch (currentTargetColor)
        {
            case fiducialColor.water:
                currentPercentage = BlackBoard.waterTiles.Count / currentTargetAmount;
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.waterTiles.Count;
                Debug.Log(currentPercentage);
                if(BlackBoard.waterTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
            case fiducialColor.sand:
                currentPercentage = BlackBoard.sandTiles.Count / currentTargetAmount;
                if (BlackBoard.sandTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
            case fiducialColor.grass:
                currentPercentage = BlackBoard.grassTiles.Count / currentTargetAmount;
                if (BlackBoard.grassTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
            case fiducialColor.snow:
                currentPercentage = BlackBoard.snowTiles.Count / currentTargetAmount;
                if (BlackBoard.snowTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
            case fiducialColor.savannah:
                currentPercentage = BlackBoard.savannahTiles.Count / currentTargetAmount;
                if (BlackBoard.savannahTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
            case fiducialColor.cherryBlossom:
                currentPercentage = BlackBoard.cherryBlossomTiles.Count / currentTargetAmount;
                if (BlackBoard.cherryBlossomTiles.Count >= currentTargetAmount)
                {
                    CompleteGoal();
                }
                break;
        }
        goalMeter.fillAmount = currentPercentage;
    }

    void CompleteGoal()
    {
        //play a little animation, and move to the next task
        completeAnimationBool = true;

        completeParticle.Play();
        StartCoroutine(CompleteAnimation());
    }

    IEnumerator CompleteAnimation()
    {
        while (completeParticle.isPlaying)
        {
            yield return null;
        }
        completeAnimationBool = false;
    }
}
