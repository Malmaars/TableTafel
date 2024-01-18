using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalGoal : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public bool completeGoal;

    public Sprite[] animalIcons;

    int currentAnimal;
    public GameObject animalCircleParent;
    Image[] iconImages;

    //the animals
    public Transform[] pandas, turtles, lions, redPandas, reindeer;

    bool completeAnimationBool = false;

    // Start is called before the first frame update
    public void Initialize()
    {
        iconImages = animalCircleParent.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    public void LogicUpdate()
    {
        //rotate the circle
        animalCircleParent.transform.rotation = Quaternion.Euler(animalCircleParent.transform.rotation.eulerAngles + rotationSpeed * Time.deltaTime);
        CheckGoal();
    }

    public void OnEnabled()
    {
        animalCircleParent.SetActive(true);
    }

    public void OnDisabled()
    {
        animalCircleParent.SetActive(false);

    }

    public void SetGoal()
    {
        completeGoal = false;
        //pick a random number that represents a random animal
        animalCircleParent.transform.position = new Vector3(Random.Range(0, 10f), animalCircleParent.transform.position.y, Random.Range(0f, 10f));

        int randomAnimal = Random.Range(0, 5);
        currentAnimal = randomAnimal;

        Sprite newSprite = animalIcons[randomAnimal];

        foreach(Image img in iconImages)
        {
            img.sprite = newSprite;

            if(randomAnimal == 3)
            {
                img.rectTransform.sizeDelta = new Vector2(1, 1);
            }

            else
            {
                img.rectTransform.sizeDelta = new Vector2(0.5f, 0.5f);
            }
        }
    }

    void CheckGoal()
    {
        Transform[] animalsToCheck = null;
        //check if the animals are in range of the circle
        switch (currentAnimal)
        {
            case 0:
                animalsToCheck = pandas;
                break;
            case 1:
                animalsToCheck = turtles;
                break;
            case 2:
                animalsToCheck = lions;
                break;
            case 3:
                animalsToCheck = reindeer;
                break;
            case 4:
                animalsToCheck = redPandas;
                break;

        }

        bool allIn = true;
        foreach(Transform animal in animalsToCheck)
        {
            Vector2 animalPos = new Vector2(animal.position.x, animal.position.z);
            Vector2 CirclePos = new Vector2(animalCircleParent.transform.position.x, animalCircleParent.transform.position.z);

            if(Vector2.Distance(animalPos, CirclePos) > 5f)
            {
                allIn = false;
            }
        }

        if (allIn)
        {
            List<ParticleSystem> particles = new List<ParticleSystem>();
            foreach(Transform animal in animalsToCheck)
            {
                particles.Add(animal.GetComponentInChildren<ParticleSystem>());
            }
            //we done it. We completed the goal;
            CompleteGoal(particles.ToArray());
        }
    }

    void CompleteGoal(ParticleSystem[] particles)
    {
        completeGoal = true;
        completeAnimationBool = true;

        foreach(ParticleSystem ps in particles)
        {
            ps.Play();
        }

        StartCoroutine(CompleteAnimation(particles));
    }

    IEnumerator CompleteAnimation(ParticleSystem[] completeParticles)
    {
        while (completeAnimationBool)
        {
            foreach (ParticleSystem particle in completeParticles)
            {
                if (!particle.isPlaying)
                    completeAnimationBool = false;

                else
                {
                    completeAnimationBool = true;
                    completeGoal = true;
                }
            }
            yield return null;
        }
        yield break;
    }
}
