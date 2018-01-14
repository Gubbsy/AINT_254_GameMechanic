using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Text pointText;
    public Slider glideBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        glideBar.value = GameDataModel.GlideValue;
        pointText.text = GameDataModel.Points.ToString();
    }
}
