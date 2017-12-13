using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickUp : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {
        GameDataModel.QuantPickUp(GameDataModel.pickupTypes.Fire, true);
        gameObject.SetActive(false);
    }
}
