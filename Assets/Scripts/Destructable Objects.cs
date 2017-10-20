using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O : MonoBehaviour {

    

    private GameObject structure;

    [SerializeField]
    private int objectHealth = 20;
    private int pointsGiven;

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TakeDamage(int damageTaken)
    {
        objectHealth -= damageTaken;
        pointsGiven = damageTaken;

        if (objectHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        structure.SetActive(false);
    }
}
