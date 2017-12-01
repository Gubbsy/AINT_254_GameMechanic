using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics : MonoBehaviour {

    [SerializeField]
    private GameObject _dot;

    private int collisions;


    public float _forceValue;
    private GameObject[] _dotLine;
    private Transform _transform;
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private bool _canGlide = false;
    private float _glideValue;
    private Vector3 _glideForceIntensity;
    [SerializeField]
    private _GameManager _GM;
   


    // Use this for initialization
    void Start () {

        //assign transfrom variable to gameobject transform value and initialsie array
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        _dotLine = new GameObject[10];
        _glideValue = 40;


        //Fill array with dot gameObjects and set them to false
        for (int i = 0; i < _dotLine.Length; i++)
        {
            GameObject tempObj = Instantiate(_dot);
            tempObj.SetActive(false);
            _dotLine[i] = tempObj;
        }

	}
	

	void Update () {
        
        float forceInX;
        float forceInY;

        _GameManager.UpdateGlide(_glideValue);

        if (_canGlide)
            Glide();

        if (Input.GetMouseButton(0))
        {
         
            Vector3 characterPosition = Camera.main.WorldToScreenPoint(_transform.position);
            characterPosition.z = 0;

            _direction = (characterPosition - Input.mousePosition).normalized ;

            forceInX = (characterPosition.x - Input.mousePosition.x);
            forceInY = (characterPosition.y - Input.mousePosition.y);

            _forceValue = Mathf.Sqrt(forceInX * forceInX + forceInY * forceInY) / 30;
           
            Aim();
        }


        if (Input.GetMouseButtonUp(0))
        {
            _rigidbody.velocity = _direction * _forceValue;
            
           for (int i = 0; i < _dotLine.Length; i++)
           {
               _dotLine[i].SetActive(false);
           }

            _canGlide = true;
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

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_glideForceIntensity);
        _glideForceIntensity.x = 0;
    }

    private void Glide()
    {
        _glideForceIntensity = new Vector3(_glideValue, 0, 0);
        _glideForceIntensity *= Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal") > 0)
        {
            _glideValue -= 0.5f;

            if (_glideValue == 0)
                _canGlide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        if (collisions == 2)
            _GM.EndInvoker();
    }

}
