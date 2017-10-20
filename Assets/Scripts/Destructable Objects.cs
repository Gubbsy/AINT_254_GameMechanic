using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O : MonoBehaviour {

    

    [SerializeField]

    private int objectHealth = 20;
    private int pointsGiven;

    private PlayerPhysics plyr;

    GameObject thePlayer = GameObject.Find("Player");
  
	// Use this for initialization
	void Start () {
        thePlayer = GameObject.Find("Player");
        plyr = thePlayer.GetComponent<PlayerPhysics>();
    }
	
	// Update is called once per frame
	void onCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Player")
            TakeDamage((int) plyr._forceValue);
		
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
        gameObject.SetActive(false);
    }
}
