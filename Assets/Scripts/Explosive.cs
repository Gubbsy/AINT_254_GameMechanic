using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    private Rigidbody rb;
    private Vector3 explosionPosition;

    [SerializeField]
    LayerMask layerMask;


    /// <summary>
    /// Detonate explosive detecting all objects in range and calling DestuctableObject to apply explosive force
    /// and damage to objects in range 
    /// </summary>
    void Detonate()
    {
        explosionPosition = transform.position;
        Collider[] objectsInRange = Physics.OverlapSphere(explosionPosition, radius);

        foreach (Collider col in objectsInRange)
        {
            Debug.Log(col.gameObject.name);
        }

        foreach (Collider col in objectsInRange)
        {
            float proximity = (explosionPosition - col.transform.position).magnitude;
            float effect = 1 - (proximity / radius);
           
            if (_GameManager.dictionary.ContainsKey(col))
            {
                _GameManager.dictionary[col].Exploded(power, explosionPosition, radius, upForce, effect);
            }
        }
    }

    //If explosive is in a collision with player or object of a cedrtain velocity, detonate.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Target"))
        {
           
            if(collision.relativeVelocity.magnitude > 10)
            {
                Detonate();
                gameObject.SetActive(false);
            }
        }
    }
}