using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathing : MonoBehaviour {

    private GameObject _flames;
    private GameObject _damagedItem;

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
            Debug.Log("E pressed");
            _flames.SetActive(true);
        }
        else
        {
           _flames.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        _damagedItem = other.gameObject;
        if (_GameManager.dictionary.ContainsKey(other))
            _GameManager.dictionary[other].TakeFireDamage();
    }

    
}
