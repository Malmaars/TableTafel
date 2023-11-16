using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class has the responsibility of being able to calibrate the screen with the table
public class CalibrationManager : MonoBehaviour
{
    Camera mainCamera;

    public int calibrationFiducial;
    public FiducialController fiducialTracker;

    //0 = center, 1 = +X, 2 = -X, 3 = +Y, 4 = -Y
    public Transform[] fiducialCalibrators;

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
                    if (liftFiducial || CalibrateZoom())
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 3:
                    SetCalibrateFiducialPositions();
                    calibrating++;
                    break;
                case 4:
                    if (liftFiducial || CalibrateFiducialPositions(0))
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 5:
                    if (liftFiducial || CalibrateFiducialPositions(1))
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 6:
                    if (liftFiducial || CalibrateFiducialPositions(2))
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 7:
                    if (liftFiducial || CalibrateFiducialPositions(3))
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 8:
                    if (liftFiducial || CalibrateFiducialPositions(4))
                    {
                        //give a prompt to lift the tracker from the table
                        if (LiftTracker())
                        {
                            liftFiducial = false;
                            calibrating++;
                        }
                    }
                    break;
                case 9:
                    CalibrateFIducialAspectRatio();
                    calibrating = 0;
                    break;
            }
                
        }

        if (Input.GetKeyDown(KeyCode.C))
            StartCalibration();
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
        digitalTable.transform.position = new Vector3(newCenter.x, digitalTable.transform.position.y, newCenter.y);

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

        digitalTable.localScale += new Vector3(direction.x * speed, 0, direction.x * speed);

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

    void SetCalibrateFiducialPositions()
    {

        //first set the calibrators at the right position
        foreach (Transform calibrator in fiducialCalibrators)
        {
            calibrator.position = BlackBoard.offset;
            calibrator.gameObject.SetActive(false);
        }
        fiducialCalibrators[1].position += new Vector3(digitalTable.localScale.x, 2, 0);
        fiducialCalibrators[2].position += new Vector3(-digitalTable.localScale.x, 2, 0);
        fiducialCalibrators[3].position += new Vector3(0, 2, digitalTable.localScale.z);
        fiducialCalibrators[4].position += new Vector3(0, 2, -digitalTable.localScale.z);
    }

    bool CalibrateFiducialPositions(int whichCalibrator)
    {
        //put one square in the center, and four squares at designated points (like 0.5, or 0.8) use these to create an accurate measurement of the table

        foreach (Transform calibrator in fiducialCalibrators)
        {
            calibrator.gameObject.SetActive(false);
        }
        //then one by one match the fiducial with the calibrators

        switch (whichCalibrator)
        {
            case 0:
                //this is the center
                fiducialCalibrators[0].gameObject.SetActive(true);
                BlackBoard.center = fiducialTracker.screenPosition;
                break;
            case 1:
                fiducialCalibrators[1].gameObject.SetActive(true);
                BlackBoard.PosX = fiducialTracker.screenPosition;
                break;
            case 2:
                fiducialCalibrators[2].gameObject.SetActive(true);
                BlackBoard.NegX = fiducialTracker.screenPosition;
                break;
            case 3:
                fiducialCalibrators[3].gameObject.SetActive(true);
                BlackBoard.Posy = fiducialTracker.screenPosition;
                break;
            case 4:
                fiducialCalibrators[4].gameObject.SetActive(true);
                BlackBoard.NegY = fiducialTracker.screenPosition;
                break;

        }

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

    void CalibrateFIducialAspectRatio()
    {
        //change the aspect ratio of the camera to combat slight change in perspective from the beamer
        BlackBoard.Xratio = BlackBoard.PosX.x * 2;
        BlackBoard.YRatio = BlackBoard.Posy.y * 2;
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
