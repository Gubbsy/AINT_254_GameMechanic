using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityCheck : MonoBehaviour {
    /// <summary>
    /// Checks whether objects are rendered or not. 
    /// </summary>

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
