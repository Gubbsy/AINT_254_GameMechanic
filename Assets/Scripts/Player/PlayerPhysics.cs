using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics : MonoBehaviour {

    [SerializeField]
    private GameObject _dot;

    public int collisions;
    public int _turn;
    private static bool _canFire;

    private bool _canGlide = false;
    public float _forceValue;
    private Vector3 _glideForceIntensity;

    private GameObject[] _dotLine;
    private  Transform _transform;
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private Vector3 _playerStartPos;

    [SerializeField]
    private _GameManager _GM;

    

    


    // Use this for initialization
    void Start() {

        //assign transfrom variable to gameobject transform value and store the players starting position 
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _playerStartPos = _transform.position;

        //Intialise dot preview line
        _dotLine = new GameObject[10];

        //Set weather the player can fire
        _canFire = false;

        //Set the player current turn
        _turn = 0;


        //Fill array with dot gameObjects and set them to false
        for (int i = 0; i < _dotLine.Length; i++)
        {
            GameObject tempObj = Instantiate(_dot);
            tempObj.SetActive(false);
            _dotLine[i] = tempObj;
        }
    }


    void Update() {
       
        if (GameDataModel.PlayMode == true)
        {

            float forceInX;
            float forceInY;

            if (_canGlide)
                Glide();

            //If the player can fire
            if (_canFire == true)
            {
                //When the player clicks and drags, use the differencce between the player postion and the mouse position  on a 2D plane to calculate fire force and direction. 
                if (Input.GetMouseButton(0))
                {
                    //Get 3D character position as 2D screen plane value.
                    Vector3 characterPosition = Camera.main.WorldToScreenPoint(_transform.position);
                    characterPosition.z = 0;
                    //get direction by normalising the diffrernce between mouse and player.
                    _direction = (characterPosition - Input.mousePosition).normalized;

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
                    _canFire = false;
                }
            }
        }

        else
            return;

    }

    private void Aim()
    {
        float Vx = _direction.x * _forceValue;
        float Vy = _direction.y * _forceValue;


        for (int i = 0; i < _dotLine.Length; i++)
        {
            float t = i * 0.1f;

            _dotLine[i].transform.position = new Vector3((_transform.position.x + Vx * t),
                (_transform.position.y + Vy * t) - (0.5f * 9.81f * t * t), _transform.position.z);

            _dotLine[i].SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (GameDataModel.PlayMode == true)
        {
            _rigidbody.AddForce(_glideForceIntensity);
            _glideForceIntensity.x = 0;
        }
        else
            return;

    }

    private void Glide()
    {
        _glideForceIntensity = new Vector3(GameDataModel.GlideValue, 0, 0);
        _glideForceIntensity *= Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal") > 0)
        {
            GameDataModel.GlideValue -= 0.5f;

            if (GameDataModel.GlideValue == 0)
                _canGlide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        if (collisions == 1)
            GameDataModel.PlayMode = true;
        if (collisions == 2)
        {
            if (_turn >= 2)
            {
                Debug.Log("3rd turn called");
                _canFire = false;
                _GM.InvokeRepeating("CheckForEnd", 3.0f, 1.0f);
            }
            else {
                _turn += 1;
                Invoke("ResetPlayer", 2);
            }
        }
    }

    public static void TurnOnLaunch(){
        _canFire = true;
    }

    public void ResetPlayer() {
        _transform.position = _playerStartPos;
        GameDataModel.GlideValue = 40;
        _canFire = true;
        _rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        collisions = 0;
        Debug.Log("Collisions: " + collisions);
        Debug.Log("Turn: " + _turn);
    }


}
