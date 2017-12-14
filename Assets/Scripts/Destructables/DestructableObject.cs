﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDestructable {


    [SerializeField]
    public int _objectHealth;

    [SerializeField]
    private int _pointsMuliplier; 

    private int _pointsGiven;
    Rigidbody rb;
    ForceMode forceMode;
  
    private bool _hasExploded;
    private Explosive _explosive;

    [SerializeField]
    public bool _isOnFire;

    [SerializeField]
    Rigidbody[] structualDepen;


    //Get refernce for GameManager Script and assign rigidbody and forceMode (for explosion)
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forceMode = ForceMode.Impulse;
        _explosive = gameObject.GetComponent<Explosive>();
        //ken = GetComponentInParent<KinematicTrigger>();
    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken)
    {
        
        //Debug.Log("Object: " + gameObject.name + "Damage Taken: " + damageTaken);

        _objectHealth -= damageTaken;
        _pointsGiven = damageTaken;

        //Debug.Log("Object: " + gameObject.name + "Health: " + _objectHealth);
        //Debug.Log("Object: " + gameObject.name + "Points Given: " + _pointsGiven);

        _GameManager.AddPoints(_pointsGiven * _pointsMuliplier);

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
            _explosive.Detonate();
        }
        else
            gameObject.SetActive(false);

        Debug.Log(rb.velocity.magnitude);

        //GameDataModel.DesObjDictionary.Remove(gameObject.GetComponent<Collider>());
    }

    //Each destructable object handles there own force and damage respectivley. 
    public void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect)
    {
        _hasExploded = true;
        rb.AddExplosionForce(power, explosionPosition, radius, upForce, forceMode);
        TakeDamage((int) (power * effect));
    }
    
    public void TakeFireDamage()
    {
        StartCoroutine(FireDamage());
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.black;
        _isOnFire = true;
    }

    public void DisableKen()
    {
        rb.isKinematic = false;
    }

    public void  OnCollisionEnter(Collision other)
    {
            float contactVelocity;
            contactVelocity = rb.velocity.magnitude;
            TakeDamage((int)contactVelocity);
    }



    public IEnumerator FireDamage()
    {
        for (int i = 0; i < 20; i++)
        {
            TakeDamage(2);
            yield return new WaitForSeconds(0.5f);
        }

        _isOnFire = false;
    }

    public bool HasExploded()
    {
        return _hasExploded;
    }

    public bool isSettling()
    {
        if(gameObject.activeInHierarchy == true)
            return _isOnFire || (rb.velocity.magnitude > 0.5 && rb.velocity.magnitude < 2);
        return false;
       
    }

    //public bool isEnabled()
    //{
    //    if (gameObject.activeInHierarchy == true)
    //        return true;

    //    return false;
    //}
}

