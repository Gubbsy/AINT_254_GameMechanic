using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTrigger : MonoBehaviour {

    private Rigidbody _rb;
    private Transform _tran;

    private void Start()
    {
        _tran = gameObject.transform;
    }

    /// <summary>
    /// When parents object collider is triggered wither by collision or explosion
    /// deactivate kinematic on all children
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "explosiveObj")
        {
            int children = transform.childCount;
            for (int i = 0; i < children; i++)
            {
                _rb = _tran.GetChild(i).GetComponent<Rigidbody>();
                _rb.isKinematic = false;
            }
        }
    }
}
