using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class _GameManager : MonoBehaviour {

    //public Slider glideBar;
    private float _glideValue;

    private int points;
    public Text pointText;

	
	void Start () {
        points = 0;
       //_glideValue = 40;
       // glideBar.value = _glideValue;
    }
	
	
	void Update () {
        pointText.text = points.ToString();
       // glideBar.value = _glideValue;
    }

    public void AddPoints(int pointsAdded)
    {
        points += pointsAdded;
    }

    public void UpdateGlide(float glide)
    {
        _glideValue = glide;
    }
}
