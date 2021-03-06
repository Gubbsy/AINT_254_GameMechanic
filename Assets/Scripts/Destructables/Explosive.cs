﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject explosion;

    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce;
    public float damageRequired;
    
  
    private Vector3 _explosionPosition;
    private GameObject _PSObject;
    private Collider thisObjCol;
    [SerializeField]
    private Collider radiusCollider;


    private void Start()
    {
        _PSObject = GameObject.FindGameObjectWithTag("emptyExplosions");
        thisObjCol = GetComponent<Collider>();
    }

    /// <summary>
    /// Detonate explosive detecting all objects in range and calling DestuctableObject to apply explosive force
    /// and damage to objects in range. Add all object's colliders within the radis to an array of collsiders,
    /// for each collider withi the array check if they are in the dictionairy of destructable object, and have not been exploded already. 
    /// Then call explosive method on destructable object. 
    /// </summary>
    public void Detonate()
    {
       
        Instantiate(explosion, transform.position,transform.rotation, _PSObject.transform);

        _explosionPosition = transform.position;
        Collider[] objectsInRange = Physics.OverlapSphere(_explosionPosition, radius);
        radiusCollider.enabled = true;
               
        

        foreach (Collider col in objectsInRange)
        {
            if (col == thisObjCol)
                continue;

            float proximity = (_explosionPosition - col.transform.position).magnitude;
            float effect = 1 - (proximity / radius);
           
            if (GameDataModel.DesObjDictionary.ContainsKey(col))
            {
                IDestructable DO = GameDataModel.DesObjDictionary[col];
                if (!DO.GetHasExploded())
                {
                    DO.Exploded(power, _explosionPosition, radius, upForce, effect);
                    DO.DisableKen();
                }  
            }
        }
        gameObject.SetActive(false);
    }

}