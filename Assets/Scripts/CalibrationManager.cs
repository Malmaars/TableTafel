using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class has the responsibility of being able to calibrate the screen with the table
public class CalibrationManager : MonoBehaviour
{
    Camera mainCamera;

    public int calibrationFiducial;
    public FiducialController fiducialTracker;

    public Transform digitalTable;
    //if calibrating is 0, there is no calibration. Above 0 is calibrating, which will go through the steps counting up.
    int calibrating;
    bool liftFiducial;
    public GameObject liftFiducialVisual;
    Vector2 lastPosition;
    float calibrationtimer;
    public float timeToEndCalibration;

    //zoom
    Vector2 zoomStartPos;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
        StartCalibration();
    }

    // Update is called once per frame
    void Update()
    {
        if(calibrating > 0)
        {
            switch (calibrating)
            {
                case 1:
                    if (liftFiducial || CalibrateCenter())
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 2:
                    if (CalibrateZoom())
                    {
                        calibrating = 0;
                    }
                    break;
            }
                
        }
    }

    public void StartCalibration()
    {
        calibrating = 1;
    }

    bool CalibrateCenter()
    {
        //Use a fiducial to move the center of the camera
        //It might be easiest to use a set fiducial to achieve this. Our "calibration fiducial if you will.

        //the new position will be a general offset for everything
        Vector2 newCenter = new Vector2(0, 0);
        newCenter = new Vector2(-fiducialTracker.screenPosition.x * 10 + 5, -fiducialTracker.screenPosition.y * 10 + 5);
        BlackBoard.offset = new Vector3(newCenter.x, 0, newCenter.y);
        mainCamera.transform.position = new Vector3(-newCenter.x, mainCamera.transform.position.y, -newCenter.y);

        //if the fiducial is on the same place long enough, end the calibration.

        if (newCenter == lastPosition && fiducialTracker.isVisible) 
        {
            calibrationtimer += Time.deltaTime;
            if (calibrationtimer > timeToEndCalibration)
            {
                //end this calibration
                calibrationtimer = 0;
                liftFiducial = true;
                return true;
            }
        }
        else
        {
            calibrationtimer = 0;
        }
        lastPosition = newCenter;
        return false;
    }

    bool CalibrateZoom()
    {
        //get the current speed and direction of the fiducial, and edit the zoom based on that.
        Vector2 direction = fiducialTracker.direction;
        float speed = fiducialTracker.speed;

        //I can't test this yet, tweak it when necessary

        //change the size of the table, not the size of the camera
        //mainCamera.orthographicSize += direction.x * speed;

        digitalTable.localScale += new Vector3(direction.x * speed, 0, direction.y * speed);

        if (fiducialTracker.ScreenPosition == lastPosition && fiducialTracker.isVisible)
        {
            calibrationtimer += Time.deltaTime;
            if (calibrationtimer > timeToEndCalibration)
            {
                //end this calibration
                calibrationtimer = 0;
                liftFiducial = true;
                return true;
            }
        }
        else
        {
            calibrationtimer = 0;
        }

        lastPosition = fiducialTracker.ScreenPosition;

        return false;
    }

    void CalibrateAspectRatio()
    {
        //change the aspect ratio of the camera to combat slight change in perspective from the beamer
    }



    bool LiftTracker()
    {
        Debug.Log(fiducialTracker.isVisible);
        //show a text on the screen to lift the fiducial
        if (fiducialTracker.isVisible)
        {
            liftFiducialVisual.SetActive(true);
            return false;
        }

        else
        {
            //remove the prompt/text
            liftFiducialVisual.SetActive(false);
            liftFiducial = false;
            return true;
        }
    }
}
