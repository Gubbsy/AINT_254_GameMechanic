using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour {

    [SerializeField]
    private int _objectHealth = 20;

    [SerializeField]
    private int _pointsMuliplier; 

    private int _pointsGiven;
    Rigidbody rb;
    ForceMode forceMode;
    private _GameManager GMscript;
    public bool _hasExploded;
    private Explosive _explosive;


    //Get refernce for GameManager Script and assign rigidbody and forceMode (for explosion)
    void Start()
    {
        GameObject GameManager = GameObject.Find("GameManager");
        GMscript = GameManager.GetComponent<_GameManager>();

        rb = GetComponent<Rigidbody>();
        forceMode = ForceMode.Impulse;

        _explosive = gameObject.GetComponent<Explosive>();

    }

    //Take damage and add points to Game Manager
    public void TakeDamage(int damageTaken)
    {
       // Debug.Log(gameObject.name + " Damage Taken: " + damageTaken);

        _objectHealth -= damageTaken;
        _pointsGiven = damageTaken;

        GMscript.AddPoints(_pointsGiven * _pointsMuliplier);

        // if health is below zero then die
        if (_objectHealth < 0)
        {
            Die();
        }

    }

    void Die()
    {
        if (gameObject.tag == "explosiveObj")
        {
            _explosive = (Explosive)gameObject.GetComponent(typeof(Explosive));
            _explosive.Detonate();
        }

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
    }



    public IEnumerator FireDamage()
    {
        for (int i = 0; i < 20; i++)
        {
            TakeDamage(2);
            Debug.Log("Taken 2 fire damage! " + System.DateTime.Now.ToString("HH:MM:ss.ff"));
            yield return new WaitForSeconds(0.5f);
        }
    }
}

