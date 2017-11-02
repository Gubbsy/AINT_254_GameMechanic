using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class _GameManager : MonoBehaviour {


    public Slider glideBar;
    private float _glideValue;

    public Text pointText;
    private int points;

    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    public static Dictionary<Collider, DestructableObject> dictionary = new Dictionary<Collider, DestructableObject>();
	
	void Start () {
        points = 0;
        
        //Loop through all gameobjects in scene and add to dictionary of it has a collider and 
        // Destructable GameObject Script 
        foreach (GameObject gameObj in Object.FindObjectsOfType<GameObject>())     
        {
            Collider col = gameObj.GetComponent<Collider>();
            DestructableObject destScript = gameObj.GetComponent<DestructableObject>();

            if (col != null && destScript != null)
            {
                dictionary.Add(col, destScript);
            }
        }
    }
	
	//Update points and glider value
	void Update () {
        pointText.text = points.ToString();
        glideBar.value = _glideValue;
    }

    //Add points
    public void AddPoints(int pointsAdded)
    {
        points += pointsAdded;
    }

    //Update glide value to that used
    public void UpdateGlide(float glide)
    {
        _glideValue = glide;
    }
}
