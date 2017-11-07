using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject explosion;

    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    public float damageRequired;
  
    private Vector3 _explosionPosition;
    private GameObject _PSObject;

    private void Start()
    {
        _PSObject = GameObject.FindGameObjectWithTag("emptyExplosions");
    }


    /// <summary>
    /// Detonate explosive detecting all objects in range and calling DestuctableObject to apply explosive force
    /// and damage to objects in range 
    /// </summary>
    void Detonate()
    {
        Instantiate(explosion, transform.position,transform.rotation, _PSObject.transform);

        _explosionPosition = transform.position;
        Collider[] objectsInRange = Physics.OverlapSphere(_explosionPosition, radius);

        

        foreach (Collider col in objectsInRange)
        {
            float proximity = (_explosionPosition - col.transform.position).magnitude;
            float effect = 1 - (proximity / radius);
           
            if (_GameManager.dictionary.ContainsKey(col))
            {
                _GameManager.dictionary[col].Exploded(power, _explosionPosition, radius, upForce, effect);
            }

            
        }
    }

    //If explosive is in a collision with player or object of a cedrtain velocity, detonate.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Target"))
        {
           
            if(collision.relativeVelocity.magnitude >= damageRequired)
            {
                Detonate();
                gameObject.SetActive(false);               
            }
        }
    }

}