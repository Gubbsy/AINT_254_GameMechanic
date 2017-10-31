using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour {

    [SerializeField]

    private int _objectHealth = 20;
    private int _pointsGiven;
    Rigidbody rb;
    ForceMode forceMode;
    
    
    private _GameManager GMscript;


    //Get refernce for GameManager Script
    void Start()
    {
        GameObject GameManager = GameObject.Find("GameManager");
        GMscript = (_GameManager)GameManager.GetComponent(typeof(_GameManager));
        rb = GetComponent<Rigidbody>();
        forceMode = ForceMode.Impulse;
    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken)
    {
        Debug.Log(gameObject.name + "Damage Taken: " + damageTaken);

        _objectHealth -= damageTaken;
        _pointsGiven = damageTaken;

        GMscript.AddPoints(_pointsGiven);

        // if health is below zero then die
        if (_objectHealth < 0)
        {
            Die();
        }

    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    public void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect)
    {
        Debug.Log("Exploded called");

        rb.AddExplosionForce(power, explosionPosition, radius, upForce, forceMode);
        TakeDamage((int) (power * effect));
    }
}

