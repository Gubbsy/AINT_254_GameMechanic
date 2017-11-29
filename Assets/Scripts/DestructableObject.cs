using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDestructable {


    [SerializeField]
    private int _objectHealth = 20;

    [SerializeField]
    private int _pointsMuliplier; 

    private int _pointsGiven;
    Rigidbody rb;
    ForceMode forceMode;
  
    private bool _hasExploded;
    private Explosive _explosive;

    [SerializeField]
    Rigidbody[] structualDepen;


    //Get refernce for GameManager Script and assign rigidbody and forceMode (for explosion)
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forceMode = ForceMode.Impulse;

        _explosive = gameObject.GetComponent<Explosive>();
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
    }

    public void DisableKen()
    {
        rb.isKinematic = false;
    }

    public void  OnCollisionEnter(Collision other)
    {
            float contactVelocity;
            contactVelocity = rb.velocity.magnitude;
           // Debug.Log("Contact velocity: " + contactVelocity);

            TakeDamage((int)contactVelocity);
    }



    public IEnumerator FireDamage()
    {
        for (int i = 0; i < 20; i++)
        {
            TakeDamage(2);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public bool HasExploded()
    {
        return _hasExploded;
    }

    public bool isSettling()
    {
        return rb.velocity.magnitude > 0.5 && rb.velocity.magnitude < 2;
    }
}

