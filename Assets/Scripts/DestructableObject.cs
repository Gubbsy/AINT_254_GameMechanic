using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDamageable  {

    [SerializeField]

    private int _objectHealth = 20;
    private int _pointsGiven;
    private Rigidbody rb;
    
    private HUDManager HUD;


    //Get refernce for GameManager Script
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
        rb = GetComponent<Rigidbody>();
    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken)
    {
        Debug.Log(gameObject.name + "Damage Taken: " + damageTaken);

        _objectHealth -= damageTaken;
        _pointsGiven = damageTaken;

        HUD.AddPoints(_pointsGiven);

        // if health is below zero then die
        if (_objectHealth < 0)
        {
            Die();
        }

    }

    public void ExplosiveHit(float power, Vector3 explosionPosition, float radius, float upForce, float effect)
    {
        rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
        TakeDamage((int)(power * effect));
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
}

