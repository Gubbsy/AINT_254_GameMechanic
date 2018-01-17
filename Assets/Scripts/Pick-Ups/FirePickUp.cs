using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickUp : MonoBehaviour {

    private AudioSource _pop;

    private void Start()
    {
        _pop = gameObject.GetComponent<AudioSource>();
    }

    //On collision with fire pick-up call quantity change of pic ups method to add a fire pick up, and the diable the pick-up. 
    private void OnTriggerEnter(Collider collision)
    {
        _pop.Play();
        GameDataModel.QuantPickUp(GameDataModel.pickupTypes.Fire, true);
        gameObject.SetActive(false);
    }
}
