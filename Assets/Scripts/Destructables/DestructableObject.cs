using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDestructable {


    [SerializeField]
    public int objectHealth;

    [SerializeField]
    public int pointsMuliplier; 

    private int _pointsGiven;
    private Rigidbody _rb;
    private ForceMode _forceMode;
  
    private bool _hasExploded;
    private Explosive _explosive;

    [SerializeField]
    public bool isOnFire;

    private Transform _trans;

    private AudioSource _breakingSound;

    private GameObject _PSObject;
    private GameObject _woodExplosion;

    private GameObject _fireEffect;
    private GameObject _fireObject;



    //Get refernce for GameManager and Explosive Script and assign rigidbody and forceMode (for explosion)
    void Start()
    {
        _PSObject = GameObject.FindGameObjectWithTag("emptyExplosions");
        _fireEffect = _PSObject.transform.GetChild(1).gameObject;

        _trans = gameObject.GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _forceMode = ForceMode.Impulse;
        _explosive = gameObject.GetComponent<Explosive>();
        _breakingSound = _trans.parent.GetComponent<AudioSource>();
        _woodExplosion = _PSObject.transform.GetChild(0).gameObject;
    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken, int pointsGiven)
    {
        //Calculate points to add and add them 
        objectHealth -= damageTaken;
        _pointsGiven = pointsGiven;       
        GameDataModel.Points += (_pointsGiven * pointsMuliplier); 

        // if health is below zero then die
        if (objectHealth < 0)
        {
            Die();
        }

    }

    // If tagged as being explosive, then detonate, and disable gameobject. 
    public void Die()
    {
        _breakingSound.pitch = (Random.Range(0.6f, 0.9f));
        _breakingSound.Play();
        if (gameObject.tag == "explosiveObj")
        {
            _hasExploded = true;
            _explosive.Detonate();

        }
        else {
            Instantiate(_woodExplosion, transform.position, transform.rotation, _PSObject.transform);
            gameObject.SetActive(false);
        }
    }

    //Each destructable object handles there own force and damage respectivley. 
    public void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect)
    {
       
        _rb.AddExplosionForce(power, explosionPosition, radius, upForce, _forceMode);
        int temp = (int)(power * 0.2f * effect);
        TakeDamage(temp,temp);
    }
    
    //When object takes fire damage start the fire damage Coroutine set isOnFire to true, and change the material colour to black. 
    public void TakeFireDamage()
    {
        StartCoroutine(FireDamage());
        _fireObject = Instantiate(_fireEffect, _trans);
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.black;
        isOnFire = true;
    }

    //Diable is kinematic on object.
    public void DisableKen()
    {
        _rb.isKinematic = false;
    }
    //On collision with another object take damage relative to the contact velocity. 
    public void  OnCollisionEnter(Collision other)
    {
        float contactVelocity;
        contactVelocity = _rb.velocity.magnitude;

        if (other.gameObject.tag == "Floor")
            TakeDamage((int)(contactVelocity * contactVelocity),(int)contactVelocity);
        else
            TakeDamage((int)(contactVelocity), (int)contactVelocity);

    }

    //Fire damage Coroutine where the player takes fire dmaage every 0.5 seconds for 10 seconds. 
    public IEnumerator FireDamage()
    {
        for (int i = 0; i < 20; i++)
        {
            TakeDamage(2,2);
            yield return new WaitForSeconds(0.5f);
        }

        isOnFire = false;
        Destroy(_fireObject);
    }

    public bool GetHasExploded()
    {
        return _hasExploded;
    }

    public void SetHasExploded(bool value)
    {
        _hasExploded = value;
    }

    //Return true or false to wether the object is still moving (has a velocity between .5 and 2 as some objects may accellerate off due to explosions at the end of a turn) 
    public bool IsSettling()
    {
        if(gameObject.activeInHierarchy == true)
            return isOnFire || (_rb.velocity.magnitude > 0.5 && _rb.velocity.magnitude < 2.5);
        return false;
       
    }

}

