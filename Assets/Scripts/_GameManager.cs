using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class _GameManager : MonoBehaviour {

    private static _GameManager singleton;

    public GameObject scoreMenu;
    private ScoreMenu _scoreMenu;

    public GameObject startMenu;




    [SerializeField]
    private GameObject[] _levels;
    public int _currentLevel { private set; get; }


    public static List<Resetable> _resetables = new List<Resetable>();

    //Singleton so there is only ever one instacne of the game mamanager. 
    public _GameManager()
    {
        if (singleton != null)
            throw new System.Exception("You can only have one _Gamemanager object! Remove one instance");

        singleton = this;
        _currentLevel = 0;
    }

    //Get and instacne of the game manager
    public static _GameManager Single()
    {
        return singleton;
    }


	void Start () {
        _scoreMenu = scoreMenu.GetComponent<ScoreMenu>();
    }

    public void StartLevel()
    {
        startMenu.SetActive(false);
        foreach (CameraRig c in GameDataModel.CamRigList)
        {
            if (c.isActiveAndEnabled == true)
                c.StartCamPan();
        }

        AddToDictionary();

        GameDataModel.Points = 0;
        Time.timeScale = 1.0f;
        scoreMenu.SetActive(false);
        GameDataModel.GlideValue = 40;
    }

   
    public void CheckForEnd()
    {
        Debug.Log("Check for end called");
        foreach (KeyValuePair<Collider,IDestructable> pair in GameDataModel.DesObjDictionary)
        {
            try
            {
                    if (pair.Value.IsSettling())
                    {
                        return;
                    }
            }
            catch { }
        }
            Invoke("EndTurn", 2f);
    }

    //End turn
    public void EndTurn()
    {
        Time.timeScale = 0.0f;
        scoreMenu.SetActive(true);
        GameDataModel.PlayMode = false;
        _scoreMenu.UpateScore();

    }

    //Reset Level
    public  void ResetLevel()
    {
        for (int i = 0; i < _resetables.Count; i++)
        {
            _resetables[i].Revert();
        }
        Debug.Log("------------------------- level reset ------------------------");
        GameDataModel.DesObjDictionary.Clear();
        StartLevel();
    }

    public void NextLevel()
    {
        _levels[_currentLevel].SetActive(false);
        GameDataModel.DesObjDictionary.Clear();
        _currentLevel++;

        for (int i = 0; i < _resetables.Count; i++)
        {
            _resetables[i].Revert();
        }

        _levels[_currentLevel].SetActive(true);
        StartLevel();
    }

    private void AddToDictionary() {

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

    





}
