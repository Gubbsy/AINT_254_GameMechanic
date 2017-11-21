using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathing : MonoBehaviour {

    private GameObject _flames;
    private GameObject _damagedItem;
    private int _noFire;
    private int _noExplosive;

    [SerializeField]
    float flameTime = 1.5f;

    private bool _flameOn;

     void Start()
    {
        _flames = GameObject.FindGameObjectWithTag("FlameCollider");
        _flames.SetActive(false);
    }

     void Update()
    {
        _noFire = _GameManager.GetPickUp(pickupTypes.Fire);
       


        if (Input.GetKey(KeyCode.E) && !_flameOn && _noFire > 0)
        {
            _flameOn = true;
          
        }

        if (_flameOn == true)
        {
            _flames.SetActive(true);
            flameTime -= Time.deltaTime;
            _GameManager.QuantPickUp(pickupTypes.Fire, false);
            if (flameTime < 0)
                _flameOn = false;
        }
        else
        {
            _flames.SetActive(false);
            flameTime = 1.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        _damagedItem = other.gameObject;
        if (_GameManager.desObjDictionary.ContainsKey(other))
            _GameManager.desObjDictionary[other].TakeFireDamage();
    }

    
}
