using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics : MonoBehaviour {

    [SerializeField]
    private GameObject _dot;

    //turn variables.
    public int collisions;
    private static bool _canFire;
    //glide variables
    private bool _canGlide = false;
    public float forceValue;
    private Vector3 _glideForceIntensity;
    //firing variables.
    private GameObject[] _dotLine;
    private  Transform _transform;
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private Vector3 _playerStartPos;

    private AudioSource _audio;

    public AudioClip lauchWoohs;
    public AudioClip launchClick;
    public AudioClip ImpactThud;

    private bool _clickPlayed;

    [SerializeField]
    private _GameManager _GM;


    // Use this for initialization
    void Start() {
        _clickPlayed = false;
        _audio = gameObject.GetComponent<AudioSource>();

        //assign transfrom variable to gameobject transform value and store the players starting position 
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _playerStartPos = _transform.position;

        //Intialise dot preview line
        _dotLine = new GameObject[10];

        //Set weather the player can fire
        _canFire = false;

        //Set the player current turn
        GameDataModel.Attempts = 3;


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
                    if (!_clickPlayed)
                    {
                        _audio.clip = launchClick;
                        _audio.Play();
                        _clickPlayed = true;
                    }
                    
                    //Get 3D character position as 2D screen plane value.
                    Vector3 characterPosition = Camera.main.WorldToScreenPoint(_transform.position);
                    characterPosition.z = 0;
                    //get direction by normalising the diffrernce between mouse and player.
                    _direction = (characterPosition - Input.mousePosition).normalized;
                    // calculate force in X and Y from the difference between theplayer and mouse
                    forceInX = (characterPosition.x - Input.mousePosition.x);
                    forceInY = (characterPosition.y - Input.mousePosition.y);

                    //calculate force value using pythagorus, devided appropriatly to give desired force. 
                    forceValue = Mathf.Sqrt(forceInX * forceInX + forceInY * forceInY) / 20;
                    //call aim preview.
                    Aim();
                }


                if (Input.GetMouseButtonUp(0))
                {
                    _clickPlayed = false;
                    _audio.clip = lauchWoohs;
                    _audio.Play();
                    //apply force to layer when moue is released using the calculated values.
                    _rigidbody.velocity = _direction * forceValue;

                    //disable dot line aim preview/
                    for (int i = 0; i < _dotLine.Length; i++)
                    {
                        _dotLine[i].SetActive(false);
                    }

                    //allow the player to glide, disallow the player to glide again. 
                    _canGlide = true;
                    _canFire = false;
                }
            }
        }
        else
            return;

        if (_audio.isPlaying == false) {
            _audio.clip = ImpactThud;
        }

    }

    private void Aim()
    {
        //calculate velocity in X and Y
        float Vx = _direction.x * forceValue;
        float Vy = _direction.y * forceValue;

        //for every dot in teh array set position relative to the force in x and y.
        for (int i = 0; i < _dotLine.Length; i++)
        {
            //calculate diatcne between dots
            float t = i * 0.1f;

            //set position to x relatibe to force in x * the distacne between the dots, set position in y realative to force in y, distance between dots minus the drop created by gravity. 
            _dotLine[i].transform.position = new Vector3((_transform.position.x + Vx * t),
                (_transform.position.y + Vy * t) - (0.5f * 9.81f * t * t), _transform.position.z);

            _dotLine[i].SetActive(true);
        }
    }

    public void ResetAimBalls()
    {
        for (int i = 0; i < _dotLine.Length; i++)
        {
            _dotLine[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!_canFire)
        {
            for (int i = 0; i < _dotLine.Length; i++)
            {
                _dotLine[i].SetActive(false);
            }
        }
        //If in playmode apply forces from gliding. 
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
        //asign glide force value based on the glide value and the users horizontal input.
        _glideForceIntensity = new Vector3(GameDataModel.GlideValue, 0, 0);
        _glideForceIntensity *= Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal") > 0)
        {
            //subtract from glide left when glide is used. 
            GameDataModel.GlideValue -= 0.5f;

            if (GameDataModel.GlideValue == 0)
                _canGlide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audio.clip = ImpactThud;
        _audio.Play();
        //Tally collison - first collision is the player contacting the starting podium, 2nd collsion is the impact after firing. 
        collisions++;

        //activate playmode on first collsion. 
        if (collisions == 1)
            GameDataModel.PlayMode = true;

        //Check which turn the player is on after th second collsion. 
        if (collisions == 2)
        {
            //iIf the turn is greater or equal to 2 the player has had 3 turns and it is he end of the round. 
            if (GameDataModel.Attempts <= 1)
            {
                //disable the player bing able to fire and crepeatedly invoke the end checker every second. 
                _canFire = false;
                GameDataModel.Attempts = 0;
                _GM.InvokeRepeating("CheckForEnd", 3.0f, 1.0f);
            }
            else {
                //increment the turn and reset the player.
                GameDataModel.Attempts -= 1;
                Invoke("ResetPlayer", 2);
            }
        }
    }

    public static void TurnOnLaunch(){
        //turn on the players ability to fire. 
        _canFire = true;
    }

    public void ResetPlayer() {
        //reset all the players values.
        _transform.position = _playerStartPos;
        GameDataModel.GlideValue = 40;
        _canFire = true;
        _rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        collisions = 0;
    }


}
