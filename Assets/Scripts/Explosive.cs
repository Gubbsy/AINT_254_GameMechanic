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

   
    //Detonate explosive detecting all objects in range and apply explosive force
    void Detonate()
    {
        explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        

        foreach (Collider hit in colliders)
        {
            rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }
        }

        AreaDamageEnemies();
    }

    //Detect all objects in range of explosion and apply damage repective of the position from the blast origin.
    void AreaDamageEnemies()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider col in objectsInRange)
        {
            DestructableObject target = col.GetComponent<DestructableObject>();

            if (target != null)
            {
                
                float proximity = (explosionPosition - target.transform.position).magnitude;
                float effect = 1 - (proximity / radius);

                target.TakeDamage((int)(power * effect));
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