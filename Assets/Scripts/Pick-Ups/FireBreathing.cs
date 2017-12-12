using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathing : MonoBehaviour {

    private GameObject _flames;
    private GameObject _damagedObj;
    private int _noFire;
    
    [SerializeField]
    float flameTime = 1.5f;

    private bool _flameOn;

    //Find flame collider object
     void Start()
    {
        _flames = GameObject.FindGameObjectWithTag("FlameCollider");
        _flames.SetActive(false);
    }

    /// <summary>
    /// Get the number of fire pick-ups and if there is more
    /// than 1 pick-up in inventroy and 'E' is pressed toggle on fire.
    /// Fire is then toggled off after a set perio of time, and pick-up removed from inventory. 
    /// </summary>
     void Update()
    {
        if (GameDataModel.PlayMode == true)
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
        else
        {
            _flames.SetActive(false);
        }
           
    }

    // On collison with flames, and object is in destObjDictionary,
    // take fire damage.
    private void OnTriggerEnter(Collider other)
    {
        if (GameDataModel.DesObjDictionary.ContainsKey(other))
            GameDataModel.DesObjDictionary[other].TakeFireDamage();
    }

    
}
