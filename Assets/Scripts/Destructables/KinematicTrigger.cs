using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTrigger : MonoBehaviour {

    private Rigidbody _rb;
    private Transform _tran;
    private GameObject[] childObjects;
    [SerializeField]
    private float _minVelocity = 3;

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
        float velocity;

        try
        {
             velocity = other.GetComponent<Rigidbody>().velocity.magnitude;
        }
        catch
        {
            velocity = 0;
        }

        if ( velocity > _minVelocity)
        {
            disableKinematic();
        }
    }

    public void disableKinematic()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            _rb = _tran.GetChild(i).GetComponent<Rigidbody>();
            _rb.isKinematic = false;
        }
    }
}
