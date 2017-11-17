using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathing : MonoBehaviour {

    private GameObject _flames;
    private GameObject _damagedItem;

    [SerializeField]
    float flameTime = 1.5f;

    private bool flameOn = false;

     void Start()
    {
        _flames = GameObject.FindGameObjectWithTag("FlameCollider");
        Debug.Log(_flames.name);
        _flames.SetActive(false);
    }

     void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            flameOn = true;
        }

        if (flameOn == true)
        {
            _flames.SetActive(true);
            flameTime -= Time.deltaTime;
            if (flameTime < 0)
                flameOn = false;
        }
        else
            _flames.SetActive(false);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        _damagedItem = other.gameObject;
        if (_GameManager.desObjDictionary.ContainsKey(other))
            _GameManager.desObjDictionary[other].TakeFireDamage();
    }

    
}
