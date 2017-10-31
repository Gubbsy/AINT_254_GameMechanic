using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class _GameManager : MonoBehaviour {

    //public Slider glideBar;
    private float _glideValue;

    private int points;
    public Text pointText;

    public static Dictionary<Collider, DestructableObject> dictionary = new Dictionary<Collider, DestructableObject>();
	
	void Start () {
        points = 0;
        //_glideValue = 40;
        // glideBar.value = _glideValue;

        foreach (GameObject thing in Object.FindObjectsOfType<GameObject>())     
        {
            Collider col = thing.GetComponent<Collider>();
            DestructableObject obj = thing.GetComponent<DestructableObject>();

            if (col != null && obj != null)
            {
                dictionary.Add(col, obj);
            }
        }
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
