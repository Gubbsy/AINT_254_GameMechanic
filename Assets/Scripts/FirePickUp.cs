using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickUp : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {
        _GameManager.QuantPickUp(pickupTypes.Fire, true);
        gameObject.SetActive(false);
        Debug.LogFormat("Fire Picked Up, Total Fire Pick ups: " + _GameManager.GetPickUp(pickupTypes.Fire));
    }
}
