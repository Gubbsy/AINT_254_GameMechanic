using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class _GameManager : MonoBehaviour {

    private static _GameManager singleton;

    public GameObject scoreMenu;
    public static GameObject staticScoreMenu;

    [SerializeField]
    private GameObject[] _levels;

    public int _currentLevel { private set; get; }

    public ScoreMenu _sc;

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
        staticScoreMenu = scoreMenu;
        _sc = scoreMenu.GetComponent<ScoreMenu>();

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
        foreach (CameraRig c in GameDataModel.CamRigList)
        {
            if (c.isActiveAndEnabled == true)
                c.StartCamPan();
        }


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
        staticScoreMenu.SetActive(true);
        GameDataModel.PlayMode = false;
        _sc.UpateScore();

    }

    //Reset Level
    public  void ResetLevel()
    {
        for (int i = 0; i < _resetables.Count; i++)
        {
            _resetables[i].Revert();
        }
        Debug.Log("------------------------- level reset ------------------------");
        StartLevel();
    }

    public void NextLevel()
    {
        _levels[_currentLevel].SetActive(false);

        _currentLevel++;

        for (int i = 0; i < _resetables.Count; i++)
        {
            _resetables[i].Revert();
        }

        _levels[_currentLevel].SetActive(true);
        StartLevel();
    }

    





}
