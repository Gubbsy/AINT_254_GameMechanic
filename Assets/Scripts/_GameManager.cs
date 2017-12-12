using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enum for pick-ups (alows for addition at later date)
public enum pickupTypes {
    Fire,
}

public class _GameManager : MonoBehaviour {

    private static _GameManager singleton;

    public _GameManager()
    {
        if (singleton != null)
            throw new System.Exception("You can only have one _Gamemanager object! Remove one instance");

        singleton = this;
    }

    public static _GameManager Single()
    {
        return singleton;
    }

    ////////////////

    public  Slider glideBar;
    public GameObject scoreMenu;
    public static GameObject staticScoreMenu;
    public  Text pointText;

    [SerializeField]
    private GameObject[] _levels;

    private int _currentLevel = 0;

    public static List<Resetable> _resetables = new List<Resetable>();


	
	void Start () {
        staticScoreMenu = scoreMenu;

        StartLevel();

        //Loop through all gameobjects in scene and add to desObjDictionary of it has a collider and 
        // Destructable GameObject Script 
        foreach (GameObject gameObj in Object.FindObjectsOfType<GameObject>())     
        {
            Collider col = gameObj.GetComponent<Collider>();
            DestructableObject destScript = gameObj.GetComponent<DestructableObject>();

            if (col != null && destScript != null)
            {
                GameDataModel.DesObjDictionary.Add(col, destScript);
            }
        }
    }

    public void StartLevel()
    {
        _levels[_currentLevel].SetActive(true);
        GameDataModel.Points = 0;
        Time.timeScale = 1.0f;
        scoreMenu.SetActive(false);
        GameDataModel.GlideValue = 40;
    }


    public void EndInvoker()
    {
        InvokeRepeating("CheckForEnd", 3.0f, 1.0f);
    }

	void Update () {
        glideBar.value = GameDataModel.GlideValue;
        pointText.text = GameDataModel.Points.ToString(); 
    }

   
    public void CheckForEnd()
    {
        foreach (KeyValuePair<Collider,IDestructable> pair in GameDataModel.DesObjDictionary)
        {
            try
            {
                if (pair.Value.isSettling())
                {
                    //Debug.Log(pair.Key.gameObject.name + ": " + pair.Key.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
                    return;
                }
            }
            catch { }
        }
        Invoke("EndTurn", 2f); //EndTurn();
    }

    //Add points
    public static void AddPoints(int pointsAdded)
    {
        GameDataModel.Points += pointsAdded;
    }


    //Add/Remove pick-ups based on enuma and bool input
    public static void QuantPickUp(pickupTypes type, bool isAdding)
    {
        if (isAdding)
            switch (type)
            {
                case pickupTypes.Fire:
                    GameDataModel.NumberFirePU += 1;
                    break;
            }
        else
            switch (type)
            {
                case pickupTypes.Fire:
                    if (GameDataModel.NumberFirePU > 0)
                        GameDataModel.NumberFirePU -= 1;
                    break;
            }
    }

    //Return number of pick-ups
    public static int GetPickUp(pickupTypes type)
    {
        if (type == pickupTypes.Fire)
            return GameDataModel.NumberFirePU;

        else
            return -1;
    }


    //End turn
    public void EndTurn()
    {
        Debug.Log("End turn called");
        Time.timeScale = 0.0f;
        staticScoreMenu.SetActive(true);
        GameDataModel.PlayMode = false;
        Debug.Log("Playmode val: " + GameDataModel.PlayMode);
    }

    //Reset Level
    public  void ResetLevel()
    {
        for (int i = 0; i < _resetables.Count; i++)
        {
            _resetables[i].Revert();
        }

        StartLevel();
        Debug.Log("Playmode val: " + GameDataModel.PlayMode);
    }

}
