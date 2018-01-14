using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickUp : MonoBehaviour {

    //On collision with fire pick-up call quantity change of pic ups method to add a fire pick up, and the diable the pick-up. 
    private void OnTriggerEnter(Collider collision)
    {
        GameDataModel.QuantPickUp(GameDataModel.pickupTypes.Fire, true);
        gameObject.SetActive(false);
    }
}
