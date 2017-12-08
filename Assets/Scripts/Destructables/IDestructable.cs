using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable{

    bool isSettling();
    bool HasExploded();
    void Die();
    void TakeDamage(int damageTaken);
    void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect);
    void TakeFireDamage();
    void DisableKen();
    IEnumerator FireDamage();

}
