using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public PlayerGoal tileGoal;
    public AnimalGoal animalGoal;

    //0 is tile goal, 1 is animal goal
    int currentGoal = 0;

    // Start is called before the first frame update
    void Awake()
    {
        tileGoal.Initialize();
        animalGoal.Initialize();
        tileGoal.SetGoals();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGoal == 0)
        {
            tileGoal.LogicUpdate();

            if (tileGoal.completeGoal)
            {
                currentGoal = 1;
                tileGoal.completeGoal = false;
                animalGoal.OnEnabled();
                animalGoal.SetGoal();
            }
        }

        else if (currentGoal == 1)
        {
            animalGoal.LogicUpdate();

            if (animalGoal.completeGoal)
            {
                currentGoal = 0;
                animalGoal.completeGoal = false;
                tileGoal.SetGoals();
                animalGoal.OnDisabled();
            }
        }
    }
}
