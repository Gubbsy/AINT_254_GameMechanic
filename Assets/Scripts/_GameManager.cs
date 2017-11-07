using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GameManager : MonoBehaviour {


    public Slider glideBar;
    public GameObject scoreMenu;
    public Text pointText;

    private float _glideValue;
    private bool _endTimer;
    private float _endTurnTimer;


    private int _points;
   
    

    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    public static Dictionary<Collider, DestructableObject> dictionary = new Dictionary<Collider, DestructableObject>();
	
	void Start () {
        _points = 0;

        Time.timeScale = 1.0f;

        scoreMenu.SetActive(false);
        _endTurnTimer = 7;
        _glideValue = 40;
        
        

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
	

	void Update () {
        pointText.text = _points.ToString();
        glideBar.value = _glideValue;

        if (_endTimer)
        {
            _endTurnTimer -= Time.deltaTime;
           // Debug.Log("Count down timer" + endTurnTimer);
        }

        if (_endTurnTimer < 0)
        {
            scoreMenu.SetActive(true);
            _endTimer = false;
        }
            

    }

    //Add points
    public void AddPoints(int pointsAdded)
    {
        _points += pointsAdded;
    }

    //Update glide value to that used
    public void UpdateGlide(float glide)
    {
        _glideValue = glide;
    }

    public int GetPoints()
    {
        return _points;
    }

    public void EndTurn()
    {
        Debug.Log("EndTurn Called");
        _endTimer = true;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
