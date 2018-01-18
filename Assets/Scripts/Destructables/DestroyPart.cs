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

        if (_objectNum > 1)
            Invoke("DestroyParticles", 4f);
	}
	
	void DestroyParticles()
    {
        Destroy(gameObject);
    }
}
