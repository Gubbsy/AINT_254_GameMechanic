using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  interface IDamageable
{
    void TakeDamage(int damage);
    void ExplosiveHit(float power, Vector3 explosionPosition, float radius, float upForce, float effect);
    Vector3 GetPosition();
}

