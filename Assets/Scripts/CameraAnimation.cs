using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
	}
	
	
    public void endAnimation()
    {
        player.SetActive(true);
    }
	
}
