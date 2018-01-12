using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

    [SerializeField]
    private Transform[] _camExtremPoints;

    [SerializeField]
    private Transform _objectiveCenter;

    [SerializeField]
    private Transform _levelCenter;

    Vector3 _camStartPos;

    [SerializeField]
    Transform _camTrans;

    private void Awake()
    {
        GameDataModel.CamRigList.Add(this);
    }

    // Use this for initialization
    void Start ()
    {
        _camStartPos = _camTrans.position;
    }

    public void StartCamPan()
    {
        StartCoroutine(cameraSlerp());
    }
	

    IEnumerator cameraSlerp()
    {
        Vector3 previousPos = _camStartPos;

        Quaternion startRot = _camTrans.rotation;
        _camTrans.LookAt(_levelCenter.transform);
        Quaternion endRot = _camTrans.rotation;



        for (int i = 0; i < _camExtremPoints.Length; i++)
        {
            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 1f) //0.5f
            {
               

                _camTrans.position = Vector3.Slerp(previousPos, _camExtremPoints[i].transform.position, j);
                
                if (i == _camExtremPoints.Length -1)
                    _camTrans.rotation = Quaternion.Slerp(startRot, endRot, j);
                else
                    _camTrans.LookAt(_objectiveCenter.transform.position);

                yield return new WaitForSeconds(0.01f);
            }
            previousPos = _camExtremPoints[i].transform.position;

            if (i == _camExtremPoints.Length - 2)
            {
                startRot = _camTrans.rotation;
                _camTrans.position = _camExtremPoints[_camExtremPoints.Length - 1].transform.position;
                _camTrans.LookAt(_levelCenter.transform);
                endRot = _camTrans.rotation;
            }
        }

        bool allRendered = false;
        Vector3 nextPosition;

        while (!allRendered)
        {
            
            nextPosition = _camTrans.position - (_levelCenter.transform.position - _camTrans.position).normalized;

            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 20f) //20f
            {
                _camTrans.LookAt(_levelCenter.transform.position);

                _camTrans.position = Vector3.Lerp(previousPos, nextPosition, j);
                yield return new WaitForSeconds(0.001f);
            }
            previousPos = _camTrans.position;

            allRendered = true;

            for (int x = 0; x < _camExtremPoints.Length; x++)
            {
                if (!_camExtremPoints[x].GetComponent<VisibilityCheck>()._isRendered)
                    allRendered = false;
            }
        }
        PlayerPhysics.TurnOnLaunch();
    }    
}
