using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class foliage : IPoolable
{
    public foliage(GameObject _visual, fiducialColor _foliageColor)
    {
        visual = _visual;
        foliageColor = _foliageColor;
    }

    public fiducialColor foliageColor;
    public GameObject visual { get; set; }
    public bool active { get; set; }
    public void OnEnableObject()
    {
        visual.SetActive(true);
    }
    public void OnDisableObject()
    {
        visual.SetActive(false);
    }
}

public class FoliageGenerator : MonoBehaviour
{
    public GameObject[] grassFoliagePrefabs;
    public GameObject[] waterFoliagePrefabs;
    public GameObject[] sandFoliagePrefabs;

    int currentGrassFoliage = 0;
    int currentWaterFoliage = 0;
    int currentSandFoliage = 0;

    ObjectPool<foliage> grassFoliage;
    ObjectPool<foliage> waterFoliage;
    ObjectPool<foliage> sandFoliage;

    private void Start()
    {
        grassFoliage = new ObjectPool<foliage>();
        waterFoliage = new ObjectPool<foliage>();
        sandFoliage = new ObjectPool<foliage>();
    }

    public foliage GenerateFoliage(fiducialColor _color, Vector3 _position)
    {
        Debug.Log("generate Foliage");
        foliage newFoliage = null;
        switch (_color)
        {
            case fiducialColor.blue:
                if (waterFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = waterFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, waterFoliagePrefabs[currentWaterFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(waterFoliagePrefabs[currentWaterFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(waterFoliagePrefabs[currentWaterFoliage], _position, newRotation), fiducialColor.blue);
                    currentWaterFoliage++;
                    if (currentWaterFoliage >= waterFoliagePrefabs.Length)
                        currentWaterFoliage = 0;
                    waterFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.red:
                if (grassFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = grassFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0,newFoliage.visual.transform.position.y,0);
                }
                else
                {
                    _position += new Vector3(0, grassFoliagePrefabs[currentGrassFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(grassFoliagePrefabs[currentGrassFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(grassFoliagePrefabs[currentGrassFoliage], _position, newRotation), fiducialColor.red);
                    currentGrassFoliage++;
                    if (currentGrassFoliage >= grassFoliagePrefabs.Length)
                        currentGrassFoliage = 0;
                    grassFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.yellow:
                if (sandFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = sandFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, sandFoliagePrefabs[currentSandFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(sandFoliagePrefabs[currentSandFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(sandFoliagePrefabs[currentSandFoliage], _position, newRotation), fiducialColor.yellow);
                    currentSandFoliage++;
                    if (currentSandFoliage >= sandFoliagePrefabs.Length)
                        currentSandFoliage = 0;
                    sandFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
        }

        return newFoliage;
    }

    public void RemoveFoliage(foliage _toRemove)
    {
        switch (_toRemove.foliageColor)
        {
            case fiducialColor.blue:
                waterFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.red:
                grassFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.yellow:
                sandFoliage.ReturnObjectToPool(_toRemove);
                break;
        }
    }
}
