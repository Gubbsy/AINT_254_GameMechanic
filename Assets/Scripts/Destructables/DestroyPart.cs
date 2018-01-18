using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPart : MonoBehaviour {

    static int _objectNum;

	// Use this for initialization
	void Start () {
        if (_objectNum == 0)
            _objectNum = 1;
        else _objectNum++;

        if (_objectNum > 2)
            Invoke("DestroyParticles", 5f);
	}
	
	public void DestroyParticles()
    {
        Destroy(gameObject);
    }
}
