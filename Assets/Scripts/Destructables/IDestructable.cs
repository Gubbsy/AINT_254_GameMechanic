using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable{

    //Interface that destructable objects must implememnt. 
    bool IsSettling();
    bool GetHasExploded();
    void SetHasExploded(bool val);
    void Die();
    void TakeDamage(int damageTaken, int pointsGiven);
    void Exploded(float power, Vector3 explosionPosition, float radius, float upForce, float effect);
    void TakeFireDamage();
    void DisableKen();
    IEnumerator FireDamage();

}
