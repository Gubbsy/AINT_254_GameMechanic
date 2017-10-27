using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Ups : MonoBehaviour {

    public GameObject fireBall;
    public GameObject PU_UseSpawn;

    private GameObject[] _fireBalls;
    private GameObject _granade;

    

	// Use this for initialization
	void Start () {

        _fireBalls = new GameObject[5];

        for(int i =0; i < _fireBalls.Length; i++)
        {
            GameObject tempObj = Instantiate(fireBall);
            tempObj.SetActive(false);
            _fireBalls[i] = tempObj;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
