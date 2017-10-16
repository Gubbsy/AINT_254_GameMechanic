using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    [SerializeField]
    private GameObject _dot;

    //[SerializeField]

    private float _forceValue;
    private Vector3 _force;

    private GameObject[] _dotLine;
    private Transform _transform;
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private Vector3 _mouseDownPos;
    

	// Use this for initialization
	void Start () {

        //assign transfrom variable to gameobject transform value and initialsie array
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        _dotLine = new GameObject[10];


        //Fill array with dot gameObjects and set them to false
        for (int i = 0; i < _dotLine.Length; i++)
        {
            GameObject tempObj = Instantiate(_dot);
            tempObj.SetActive(false);
            _dotLine[i] = tempObj;
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        //On mouse click calcualte direction of projectile using character position and mouse postion. 

        float forceInX;
        float forceInY;
        

        if (Input.GetMouseButton(0))
        {
            _mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseDownPos.z = 0;
            _direction = (_mouseDownPos - Input.mousePosition).normalized ;

            forceInX = (_mouseDownPos.x - Input.mousePosition.x);
            forceInY = (_mouseDownPos.y - Input.mousePosition.y);

            //Drag distance calculation a^2 = b^2 + c^2
            _forceValue = Mathf.Sqrt(forceInX * forceInX + forceInY * forceInY) / 100;

            Debug.Log(string.Format("Direction value: {0}, Force Value: {1}", _direction, _forceValue));

            Aim();
        }


        if (Input.GetMouseButtonUp(0))
        {
            _rigidbody.velocity = _direction * _forceValue;
            
           for (int i = 0; i < _dotLine.Length; i++)
           {
               _dotLine[i].SetActive(false);
           }
        }

    }

    private void Aim()
    {
        float Vx = _direction.x * _forceValue;
        float Vy = _direction.y * _forceValue;

        for (int i = 0; i < _dotLine.Length; i++)
        {
            float t = i * 0.1f;

            _dotLine[i].transform.position = new Vector3((_transform.position.x + Vx * t), 
                (_transform.position.y + Vy * t) - (0.5f * 9.81f * t*t), 0.0f);

            _dotLine[i].SetActive(true);
        }
    }
}
