﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enum for pick-ups (alows for addition at later date)
public enum pickupTypes {
    Fire,
}

public class _GameManager : MonoBehaviour {

    public  Slider glideBar;
    public  GameObject scoreMenu;
    public  Text pointText;

    private static float _glideValue;
    private static bool _endTimer;
    private static float _endTurnTimer;
    private static int _noFire;
    private static int _noExplosives;
    private static int _points;

    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    public static Dictionary<Collider, DestructableObject> desObjDictionary = new Dictionary<Collider, DestructableObject>();
	
	void Start () {
        _points = 0;

        Time.timeScale = 1.0f;

        scoreMenu.SetActive(false);
        _endTurnTimer = 7;
        _glideValue = 40;
        

        //Loop through all gameobjects in scene and add to desObjDictionary of it has a collider and 
        // Destructable GameObject Script 
        foreach (GameObject gameObj in Object.FindObjectsOfType<GameObject>())     
        {
            Collider col = gameObj.GetComponent<Collider>();
            DestructableObject destScript = gameObj.GetComponent<DestructableObject>();

            if (col != null && destScript != null)
            {
                desObjDictionary.Add(col, destScript);
            }
        }
    }
	

	void Update () {
        pointText.text = _points.ToString();
        glideBar.value = _glideValue;

        if (_endTimer)
        {
            _endTurnTimer -= Time.deltaTime;
        }

        if (_endTurnTimer < 0)
        {
            scoreMenu.SetActive(true);
            _endTimer = false;
        }
            

    }

    //Add points
    public static void AddPoints(int pointsAdded)
    {
        _points += pointsAdded;
    }

    //Update glide value to that used
    public static void UpdateGlide(float glide)
    {
        _glideValue = glide;
    }

    //Add/Remove pick-ups based on enuma and bool input
    public static void QuantPickUp(pickupTypes type, bool isAdding)
    {
        if (isAdding)
            switch (type)
            {
                case pickupTypes.Fire:
                    _noFire += 1;
                    break;
            }
        else
            switch (type)
            {
                case pickupTypes.Fire:
                    if (_noFire > 0)
                        _noFire -= 1;
                    break;
            }
    }

    //Return number of pick-ups
    public static int GetPickUp(pickupTypes type)
    {
        if (type == pickupTypes.Fire)
            return _noFire;

        else
            return -1;
    }

    //Get points 
    public static int GetPoints()
    {
        return _points;
    }

    //End turn
    public static void EndTurn()
    {
        Debug.Log("EndTurn Called");
        _endTimer = true;
    }

    //Reset Level
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
