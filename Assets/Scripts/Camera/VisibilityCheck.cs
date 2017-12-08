using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityCheck : MonoBehaviour {

    public bool _isRendered { get; set; }

    private void OnBecameVisible()
    {
        _isRendered = true;
    }

    private void OnBecameInvisible()
    {
        _isRendered = false;
    }

}
