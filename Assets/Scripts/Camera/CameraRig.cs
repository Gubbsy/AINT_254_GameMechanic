using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

    //Array o gameobject transforms of the extreme points placed in teh scene for camera to pivot around. 
    [SerializeField]
    private Transform[] _camExtremPoints;
    //center objective point the camera to focus on 
    [SerializeField]
    private Transform _objectiveCenter;
    //center of level poit for camaera to snap to and pan out from.
    [SerializeField]
    private Transform _levelCenter;

    //camera start position
    private Vector3 _camStartPos;

    //cameras transfrom component. 
    [SerializeField]
    Transform _camTrans;

    private void Awake()
    {
        //Add thi camera rig to the list of possible camera rigs. 
        GameDataModel.CamRigList.Add(this);
        _camStartPos = _camTrans.position;
    }

    
    void Start ()
    {
        
    }

    public void StartCamPan()
    {   //start cam rig code rouine to begin camera movement. 
        StartCoroutine(cameraSlerp());
    }
	

    IEnumerator cameraSlerp()
    {
        //assign values of postion, rotaion of camera to intila position values. 
        Vector3 previousPos = _camStartPos;

        Quaternion startRot = _camTrans.rotation;
        _camTrans.LookAt(_levelCenter.transform);
        Quaternion endRot = _camTrans.rotation;


        // for every extreme point defined slerp the camera position and rotation, over time j
        for (int i = 0; i < _camExtremPoints.Length; i++)
        {
            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 1f) //0.5f
            {
                _camTrans.position = Vector3.Slerp(previousPos, _camExtremPoints[i].transform.position, j);
                
                //if this is the last extreme point, camera sleps between the start and end rotation. 
                //else look at the centre point
                if (i == _camExtremPoints.Length -1)
                    _camTrans.rotation = Quaternion.Slerp(startRot, endRot, j);
                else
                    _camTrans.LookAt(_objectiveCenter.transform.position);

                yield return new WaitForSeconds(0.01f);
            }
            //incrmement the previouse position 
            previousPos = _camExtremPoints[i].transform.position;

            //if this is the 2nd to last extreme point, asssign th start rotation to the current rotation, assign the cammera position to the last position,
            // and have camera look at the centre point, assigning this to be the end rotation. 
            if (i == _camExtremPoints.Length - 2)
            {
                startRot = _camTrans.rotation;
                _camTrans.position = _camExtremPoints[_camExtremPoints.Length - 1].transform.position;
                _camTrans.LookAt(_levelCenter.transform);
                endRot = _camTrans.rotation;
            }
        }

        //if all objects are not rendered have the camera pan backwards until all objects are rendered. 
        bool allRendered = false;
        Vector3 nextPosition;

        while (!allRendered)
        {
            //caculate directin of camera pan
            nextPosition = _camTrans.position - (_levelCenter.transform.position - _camTrans.position).normalized;

            //camera moves backwards, looking at center, laerping from next previouse postion to nect postion over time f j.
            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 20f) //20f
            {
                _camTrans.LookAt(_levelCenter.transform.position);

                _camTrans.position = Vector3.Lerp(previousPos, nextPosition, j);
                yield return new WaitForSeconds(0.001f);
            }
            //assign previouse positon to current postion
            previousPos = _camTrans.position;

            //assign all rendered to true.
            allRendered = true;

            //check of all the extreme points are rendered by teh camera
            for (int x = 0; x < _camExtremPoints.Length; x++)
            {
                if (!_camExtremPoints[x].GetComponent<VisibilityCheck>()._isRendered)
                    allRendered = false;
            }
        }
        //turn on player launch once camera pan is complete. 
        PlayerPhysics.TurnOnLaunch();
    }    
}
