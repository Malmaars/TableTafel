using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGoal : MonoBehaviour
{
    //This script is to give the players a goal, and reward them for completing that goal
    //the goal will consist of mutliple types of tiles, asking for specific amounts

    public Sprite[] TileSprites;

    public GameObject TileGoalPrefab;

    bool completeAnimationBool = false;
    public ParticleSystem[] completeParticles;

    List<Goal> activegoals;
    List<Goal> inactiveGoals;

    // Start is called before the first frame update
    void Start()
    {
        activegoals = new List<Goal>();
        inactiveGoals = new List<Goal>();

        Goal firstGoal = new Goal(TileGoalPrefab, transform);
        firstGoal.Initialize(new Vector3(1, 0, 1));
        activegoals.Add(firstGoal);
        CreateGoal(firstGoal, fiducialColor.water, 500);

        Goal secondGoal = new Goal(TileGoalPrefab, transform);
        secondGoal.Initialize(new Vector3(-1, 0, 1));
        activegoals.Add(secondGoal);
        CreateGoal(secondGoal, fiducialColor.grass, 500);

        Goal thirdGoal = new Goal(TileGoalPrefab, transform);
        thirdGoal.Initialize(new Vector3(1, 0, -1));
        activegoals.Add(thirdGoal);
        CreateGoal(thirdGoal, fiducialColor.sand, 500);

        Goal fourthGoal = new Goal(TileGoalPrefab, transform);
        fourthGoal.Initialize(new Vector3(-1, 0, -1));
        activegoals.Add(fourthGoal);
        CreateGoal(fourthGoal, fiducialColor.snow, 500);

    }

    // Update is called once per frame
    void Update()
    {
        bool completedGoal = true;
        foreach(Goal goal in activegoals)
        {
            goal.CheckGoal();
            if (!goal.goalComplete)
            {
                completedGoal = false;
            }
        }
        if (completedGoal && activegoals.Count > 0)
        {
            CompleteGoal();
            for (int i = 0; i < activegoals.Count; i+= 0)
            {
                activegoals[i].OnDisableObject();
                inactiveGoals.Add(activegoals[i]);
                activegoals.Remove(activegoals[i]);
            }
        }
    }

    void CreateGoal(Goal newGoal, fiducialColor newTile, int goalAmount)
    {
        Sprite newSprite = null;
        //replace the sprite with the sprite of the new tile type
        switch (newTile)
        {
            case fiducialColor.water:
                newSprite = TileSprites[0];
                break;
            case fiducialColor.sand:
                newSprite = TileSprites[1];
                break;
            case fiducialColor.grass:
                newSprite = TileSprites[2];
                break;
            case fiducialColor.snow:
                newSprite = TileSprites[3];
                break;
            case fiducialColor.savannah:
                newSprite = TileSprites[4];
                break;
            case fiducialColor.cherryBlossom:
                newSprite = TileSprites[5];
                break;
        }

        newGoal.SetGoal(newTile, goalAmount,newSprite);
    }

    

    void CompleteGoal()
    {
        //play a little animation, and move to the next task
        completeAnimationBool = true;

        foreach(ParticleSystem particle in completeParticles)
        {
            particle.Play();
        }

        StartCoroutine(CompleteAnimation());
    }

    IEnumerator CompleteAnimation()
    {
        while (completeAnimationBool)
        {
            foreach (ParticleSystem particle in completeParticles)
            {
                if (!particle.isPlaying)
                    completeAnimationBool = false;
                
                else
                    completeAnimationBool = true;
            }
            yield return null;
        }
        yield break;
    }
}

public class Goal : IPoolable
{
    public bool active { get; set; }
    public void OnEnableObject()
    {
        goalComplete = false;
        tileObject.SetActive(true);
    }
    public void OnDisableObject()
    {
        tileObject.SetActive(false);
    }
    public Transform parent;
    public GameObject prefab;

    fiducialColor currentTargetColor = fiducialColor.wasteland;
    int currentTargetAmount;

    GameObject tileObject;
    SpriteRenderer tileVisual;
    public Image goalMeter;

    public bool goalComplete;

    public Goal(GameObject _prefab, Transform _parent)
    {
        prefab = _prefab;
        parent = _parent;
    }

    public void Initialize(Vector3 _offset)
    {
        tileObject = Object.Instantiate(prefab, parent);
        tileObject.transform.position += _offset;
        tileVisual = tileObject.GetComponentInChildren<SpriteRenderer>();
        goalMeter = tileObject.GetComponentsInChildren<Image>()[2];
    }

    public void SetGoal(fiducialColor newTile, int targetAmount, Sprite newSprite)
    {
        if (newTile == currentTargetColor)
        {
            Debug.LogWarning("Newtile shouldn't be the same as the completed tile mission");
            return;
        }

        tileVisual.sprite = newSprite;
        currentTargetColor = newTile;
        currentTargetAmount = targetAmount;
    }

    public void CheckGoal()
    {
        float currentPercentage = 0;
        switch (currentTargetColor)
        {
            case fiducialColor.water:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.waterTiles.Count;
                if (BlackBoard.waterTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
            case fiducialColor.sand:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.sandTiles.Count;
                if (BlackBoard.sandTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
            case fiducialColor.grass:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.grassTiles.Count;
                if (BlackBoard.grassTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
            case fiducialColor.snow:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.snowTiles.Count;
                if (BlackBoard.snowTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
            case fiducialColor.savannah:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.savannahTiles.Count;
                if (BlackBoard.savannahTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
            case fiducialColor.cherryBlossom:
                currentPercentage = 1 / (float)currentTargetAmount * (float)BlackBoard.cherryBlossomTiles.Count;
                if (BlackBoard.cherryBlossomTiles.Count >= currentTargetAmount)
                {
                    goalComplete = true;
                }
                else
                {
                    goalComplete = false;
                }
                break;
        }
        goalMeter.fillAmount = currentPercentage;
    }
}
