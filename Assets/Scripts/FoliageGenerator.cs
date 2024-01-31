using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class foliage : IPoolable
{
    public foliage(GameObject _visual, fiducialColor _foliageColor)
    {
        visual = _visual;
        foliageColor = _foliageColor;
        originalSize = visual.transform.localScale;
    }

    public Vector3 originalSize;

    public fiducialColor foliageColor;
    public GameObject visual { get; set; }

    public bool beingAnimated;
    public int controllers;

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
    public GameObject[] snowFoliagePrefabs;
    public GameObject[] bambooFoliagePrefabs;
    public GameObject[] savannahFoliagePrefabs;
    public GameObject[] cherryBlossomFoliagePrefabs;

    int currentGrassFoliage = 0;
    int currentWaterFoliage = 0;
    int currentSandFoliage = 0;
    int currentSnowFoliage = 0;
    int currentSavannahFoliage = 0;
    int currentBambooFoliage = 0;
    int currentCherryBlossomFoliage = 0;

    ObjectPool<foliage> grassFoliage;
    ObjectPool<foliage> waterFoliage;
    ObjectPool<foliage> sandFoliage;
    ObjectPool<foliage> snowFoliage;
    ObjectPool<foliage> bambooFoliage;
    ObjectPool<foliage> savannahFoliage;
    ObjectPool<foliage> cherryBlossomFoliage;

    private void Start()
    {
        grassFoliage = new ObjectPool<foliage>();
        waterFoliage = new ObjectPool<foliage>();
        sandFoliage = new ObjectPool<foliage>();
        snowFoliage = new ObjectPool<foliage>();
        bambooFoliage = new ObjectPool<foliage>();
        savannahFoliage = new ObjectPool<foliage>();
        cherryBlossomFoliage = new ObjectPool<foliage>();
    }

    public foliage GenerateFoliage(fiducialColor _color, Vector3 _position)
    {
        //Debug.Log("generate Foliage");
        foliage newFoliage = null;
        switch (_color)
        {
            case fiducialColor.water:
                if (waterFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = waterFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, waterFoliagePrefabs[currentWaterFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(waterFoliagePrefabs[currentWaterFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(waterFoliagePrefabs[currentWaterFoliage], _position, newRotation), fiducialColor.water);
                    currentWaterFoliage++;
                    if (currentWaterFoliage >= waterFoliagePrefabs.Length)
                        currentWaterFoliage = 0;
                    waterFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.grass:
                if (grassFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = grassFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0,newFoliage.visual.transform.position.y,0);
                }
                else
                {
                    _position += new Vector3(0, grassFoliagePrefabs[currentGrassFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(grassFoliagePrefabs[currentGrassFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(grassFoliagePrefabs[currentGrassFoliage], _position, newRotation), fiducialColor.grass);
                    currentGrassFoliage++;
                    if (currentGrassFoliage >= grassFoliagePrefabs.Length)
                        currentGrassFoliage = 0;
                    grassFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.sand:
                if (sandFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = sandFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, sandFoliagePrefabs[currentSandFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(sandFoliagePrefabs[currentSandFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(sandFoliagePrefabs[currentSandFoliage], _position, newRotation), fiducialColor.sand);
                    currentSandFoliage++;
                    if (currentSandFoliage >= sandFoliagePrefabs.Length)
                        currentSandFoliage = 0;
                    sandFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.snow:
                if (snowFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = snowFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, snowFoliagePrefabs[currentSnowFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(snowFoliagePrefabs[currentSnowFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(snowFoliagePrefabs[currentSnowFoliage], _position, newRotation), fiducialColor.snow);
                    currentSnowFoliage++;
                    if (currentSnowFoliage >= snowFoliagePrefabs.Length)
                        currentSnowFoliage = 0;
                    snowFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.bamboo:
                if (bambooFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = bambooFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, bambooFoliagePrefabs[currentBambooFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(bambooFoliagePrefabs[currentBambooFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(bambooFoliagePrefabs[currentBambooFoliage], _position, newRotation), fiducialColor.bamboo);
                    currentBambooFoliage++;
                    if (currentBambooFoliage >= bambooFoliagePrefabs.Length)
                        currentBambooFoliage = 0;
                    bambooFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.savannah:
                if (savannahFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = savannahFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, savannahFoliagePrefabs[currentSavannahFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(savannahFoliagePrefabs[currentSavannahFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(savannahFoliagePrefabs[currentSavannahFoliage], _position, newRotation), fiducialColor.savannah);
                    currentSavannahFoliage++;
                    if (currentSavannahFoliage >= savannahFoliagePrefabs.Length)
                        currentSavannahFoliage = 0;
                    savannahFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
            case fiducialColor.cherryBlossom:
                if (cherryBlossomFoliage.RequestPoolSize() > 0)
                {
                    newFoliage = cherryBlossomFoliage.RequestItem();
                    newFoliage.visual.transform.position = _position + new Vector3(0, newFoliage.visual.transform.position.y, 0);
                }
                else
                {
                    _position += new Vector3(0, cherryBlossomFoliagePrefabs[currentCherryBlossomFoliage].transform.position.y, 0);
                    Quaternion newRotation = Quaternion.Euler(new Vector3(cherryBlossomFoliagePrefabs[currentCherryBlossomFoliage].transform.rotation.eulerAngles.x, 0, Random.Range(0, 360f)));
                    newFoliage = new foliage(Instantiate(cherryBlossomFoliagePrefabs[currentCherryBlossomFoliage], _position, newRotation), fiducialColor.cherryBlossom);
                    currentCherryBlossomFoliage++;
                    if (currentCherryBlossomFoliage >= cherryBlossomFoliagePrefabs.Length)
                        currentCherryBlossomFoliage = 0;
                    cherryBlossomFoliage.AddActiveItemToPool(newFoliage);
                }
                break;
        }

        StartCoroutine(AnimateFoliage(newFoliage));
        return newFoliage;
    }

    public IEnumerator RemoveFoliage(foliage _toRemove)
    {

        while (_toRemove.beingAnimated)
        {
            //wait
            yield return null;
        }

        Vector3 target = _toRemove.originalSize * 2;
        _toRemove.visual.transform.position += new Vector3(0, 1, 0);
        while (Vector3.Distance(_toRemove.visual.transform.localScale, target) > 0.1f)
        {
            _toRemove.visual.transform.localScale = Vector3.Lerp(_toRemove.visual.transform.localScale, target, Time.deltaTime * 20);
            yield return null; // Pause and resume on the next frame
        }

        while (_toRemove.visual.transform.localScale != Vector3.zero)
        {
            _toRemove.visual.transform.localScale = Vector3.Lerp(_toRemove.visual.transform.localScale, Vector3.zero, Time.deltaTime * 40);
            yield return null; // Pause and resume on the next frame
        }
        _toRemove.visual.transform.position -= new Vector3(0, 1, 0);
        _toRemove.visual.transform.localScale = Vector3.zero;

        switch (_toRemove.foliageColor)
        {
            case fiducialColor.water:
                waterFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.grass:
                grassFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.sand:
                sandFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.snow:
                snowFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.bamboo:
                bambooFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.savannah:
                savannahFoliage.ReturnObjectToPool(_toRemove);
                break;
            case fiducialColor.cherryBlossom:
                cherryBlossomFoliage.ReturnObjectToPool(_toRemove);
                break;
        }
    }

    IEnumerator AnimateFoliage(foliage _foliage)
    {
        while (_foliage.beingAnimated)
        {
            //wait
            yield return null;
        }

        _foliage.controllers++;
        _foliage.beingAnimated = true;
        Vector3 oldTransform = _foliage.originalSize;
        
        Vector3 target = oldTransform * 2;
        _foliage.visual.transform.position += new Vector3(0, 1, 0);
        while (_foliage.visual.transform.localScale.x < target.x - 0.01f)
        {
            _foliage.visual.transform.localScale = Vector3.Lerp(_foliage.visual.transform.localScale, target, Time.deltaTime * 30);
            yield return null; // Pause and resume on the next frame
        }

        while (_foliage.visual.transform.localScale.x > oldTransform.x + 0.01f)
        {
            Vector3 newScale = Vector3.Lerp(_foliage.visual.transform.localScale, oldTransform, Time.deltaTime * 10);

            if (newScale.x < _foliage.visual.transform.localScale.x)
            {
                _foliage.visual.transform.localScale = newScale;
            }
            else
            {
                Debug.Log("ITS BIGGER HOW IS IT BIGGER");
            }
            //yield return null; // Pause and resume on the next frame
            yield return new WaitForEndOfFrame();
        }

        _foliage.controllers = 0;
        _foliage.visual.transform.position -= new Vector3(0, 1, 0);
        _foliage.visual.transform.localScale = oldTransform;
        _foliage.beingAnimated = false;
    }
}
