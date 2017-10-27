using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;

    private Vector3 explosionPosition;
    public LayerMask explosiveLayer;
   

   
    //Detonate explosive detecting all objects in range and apply explosive force
    void Detonate()
    {
        explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius, explosiveLayer);

        foreach (Collider hit in colliders)
        {
                IDamageable tempObj = hit.GetComponent<IDamageable>();

                float proximity = (explosionPosition - tempObj.GetPosition()).magnitude;
                float effect = 1 - (proximity / radius);

                tempObj.ExplosiveHit(power, explosionPosition, radius, upForce, effect);
            
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