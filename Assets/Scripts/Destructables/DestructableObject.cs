using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDestructable {


    [SerializeField]
    public int _objectHealth;

    [SerializeField]
    private int _pointsMuliplier; 

    private int _pointsGiven;
    private Rigidbody _rb;
    private ForceMode _forceMode;
  
    private bool _hasExploded;
    private Explosive _explosive;

    [SerializeField]
    public bool isOnFire;

    [SerializeField]
    private Rigidbody[] _structualDepen;


    //Get refernce for GameManager and Explosive Script and assign rigidbody and forceMode (for explosion)
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _forceMode = ForceMode.Impulse;
        _explosive = gameObject.GetComponent<Explosive>();
    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken)
    {
        //Calculate points to add and add them 
        _objectHealth -= damageTaken;
        _pointsGiven = damageTaken;       
        GameDataModel.Points += (_pointsGiven * _pointsMuliplier); 

        // if health is below zero then die
        if (_objectHealth < 0)
        {
            Die();
        }

    }

    // If tagged as being explosive, then detonate, and disable gameobject. 
    public void Die()
    {
        if (gameObject.tag == "explosiveObj")
        {
            _hasExploded = true;
            _explosive.Detonate();
            
        }
        else
            gameObject.SetActive(false);

    }

    //Each destructable object handles there own force and damage respectivley. 
    public void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect)
    {
       
        _rb.AddExplosionForce(power, explosionPosition, radius, upForce, _forceMode);
        TakeDamage((int) (power * effect));
    }
    
    //When object takes fire damage start the fire damage Coroutine set isOnFire to true, and change the material colour to black. 
    public void TakeFireDamage()
    {
        StartCoroutine(FireDamage());
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
            TakeDamage((int)contactVelocity);
    }

    //Fire damage Coroutine where the player takes fire dmaage every 0.5 seconds for 10 seconds. 
    public IEnumerator FireDamage()
    {
        for (int i = 0; i < 20; i++)
        {
            TakeDamage(2);
            yield return new WaitForSeconds(0.5f);
        }

        isOnFire = false;
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

