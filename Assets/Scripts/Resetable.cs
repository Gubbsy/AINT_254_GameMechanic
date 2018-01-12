﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetable : MonoBehaviour
{
    [SerializeField]
    private bool _resetPosition;
    private Vector3 _position;

    [SerializeField]
    private bool _resetRotation;
    private Quaternion _rotation;

    [SerializeField]
    private bool _resetScale;
    private Vector3 _scale;

    [SerializeField]
    private bool _resetActive;
    private bool _active;


    private int _health;
    private bool _isKin;
    private Color _color;

    

    Transform _transform;
    GameObject _gameObject;
    Rigidbody _rb;
    DestructableObject _do;
    PlayerPhysics _playerPhysics;
    Renderer _ren;
    FireBreathing _fireBreathing;

  
    private void Awake()
    {
        _gameObject = gameObject;
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _do = GetComponent<DestructableObject>();
        _ren = GetComponent<Renderer>();
        _playerPhysics = GetComponent<PlayerPhysics>();
        _fireBreathing = GetComponent<FireBreathing>(); 
        Track();

        _GameManager._resetables.Add(this);
    }

    public void Revert()
    {
        if(_resetPosition)
            _transform.position = _position;
        if(_resetRotation)
            _transform.rotation = _rotation;
        if(_resetScale)
            _transform.localScale = _scale;
        if(_resetActive)
            _gameObject.SetActive(_active);
     
            



        if (_rb != null)
        {
            _rb.isKinematic = _isKin;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        if (_do != null)
        {
            _do._objectHealth = _health;
            _do._isOnFire = false;
            _do.SetHasExploded(false);
        }
            
        if (_ren != null)
            _ren.material.color = _color;

        if (_playerPhysics != null)
            _playerPhysics.collisions = 0;

        if (_fireBreathing != null) {
            _fireBreathing.flameTime = 1.5f;
        }
           

        GameDataModel.SetPickUpVal(GameDataModel.pickupTypes.Fire, 0);

        _GameManager.Single().CancelInvoke("EndTurn");
        _GameManager.Single().CancelInvoke("EndInvoker");
        _GameManager.Single().CancelInvoke("CheckForEnd");
    }

    public void Track()
    {
        _position = _transform.position;
        _rotation = _transform.rotation;
        _scale = _transform.localScale;
        _active = _gameObject.activeInHierarchy;

        if (_rb != null)
        {
            _isKin = _rb.isKinematic;
            _rb.angularVelocity = new Vector3(0,0,0);
        }
            

        if (_do != null)
        {
            _health = _do._objectHealth;
 
        }
           

        if (_ren != null)
            if (_ren.material.HasProperty("_Color"))
                _color = _ren.material.color;
    }
                
}
