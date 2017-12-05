using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {



    [SerializeField]
    private GameObject[] _camExtremPoints;

    [SerializeField]
    private GameObject _centerPoint;

    Vector3 _camStartPos;

    [SerializeField]
    Camera _mainCam;




	// Use this for initialization
	void Start () {
        _camStartPos = _mainCam.transform.position;
        StartCoroutine(cameraSlerp());
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    IEnumerator cameraSlerp()
    {
        Vector3 previousPos = _camStartPos;
      

        for (int i = 0; i < _camExtremPoints.Length; i++)
        {
            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 3)
            {
                Debug.Log("J: "+j + " | i: " + i);
               
                _mainCam.transform.LookAt(_centerPoint.transform.position);

                _mainCam.transform.position = Vector3.Slerp(previousPos, _camExtremPoints[i].transform.position, j);
                yield return new WaitForSeconds(0.05f);
            }
            previousPos = _camExtremPoints[i].transform.position;
        }

        bool allRendered = false;
        Vector3 nextPosition;

        while (!allRendered)
        {
            nextPosition = _mainCam.transform.position - (_centerPoint.transform.position - _mainCam.transform.position).normalized;

            Debug.DrawLine((_centerPoint.transform.position - _mainCam.transform.position).normalized, nextPosition, Color.blue, Mathf.Infinity);

            for (float j = 0.0f; j < 1.0f; j += Time.deltaTime * 10)
            {
                _mainCam.transform.LookAt(_centerPoint.transform.position);

                _mainCam.transform.position = Vector3.Lerp(previousPos, nextPosition, j);
                yield return new WaitForSeconds(0.05f);
            }
            previousPos = _mainCam.transform.position;

            allRendered = true;

            for (int x = 0; x < _camExtremPoints.Length; x++)
            {
                if (!_camExtremPoints[x].GetComponent<VisibilityCheck>()._isRendered)
                    allRendered = false;
            }
            Debug.Log(allRendered.ToString().ToUpper());
        }
    }    
}
