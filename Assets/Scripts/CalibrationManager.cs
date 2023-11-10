using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class has the responsibility of being able to calibrate the screen with the table
public class CalibrationManager : MonoBehaviour
{
    Camera mainCamera;

    public int calibrationFiducial;
    private FiducialController fiducialTracker;

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
        fiducialTracker = GetComponent<FiducialController>();
        fiducialTracker.MarkerID = calibrationFiducial;

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
                            calibrating++;
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
        newCenter = fiducialTracker.screenPosition;
        BlackBoard.offset = newCenter;

        //if the fiducial is on the same place long enough, end the calibration.

        if (newCenter == lastPosition && fiducialTracker.isVisible) 
        {
            calibrationtimer += Time.deltaTime;
            if (calibrationtimer > timeToEndCalibration)
            {
                //end this calibration
                calibrationtimer = 0;
                return true;
            }
        }
        else
        {
            calibrationtimer = 0;
        }
        lastPosition = fiducialTracker.screenPosition;
        return false;
    }

    bool CalibrateZoom()
    {
        //get the current speed and direction of the fiducial, and edit the zoom based on that.
        Vector2 direction = fiducialTracker.direction;
        float speed = fiducialTracker.speed;

        //I can't test this yet, tweak it when necessary
        mainCamera.orthographicSize += direction.x * speed;

        if (fiducialTracker.screenPosition == lastPosition && fiducialTracker.isVisible)
        {
            calibrationtimer += Time.deltaTime;
            if (calibrationtimer > timeToEndCalibration)
            {
                //end this calibration
                calibrationtimer = 0;
                return true;
            }
        }
        else
        {
            calibrationtimer = 0;
        }

        return false;
    }

    void CalibrateAspectRatio()
    {
        //change the aspect ratio of the camera to combat slight change in perspective from the beamer
    }

    bool LiftTracker()
    {

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