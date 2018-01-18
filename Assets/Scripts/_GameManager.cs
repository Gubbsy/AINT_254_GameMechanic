using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class _GameManager : MonoBehaviour {

    private static _GameManager singleton;

    public GameObject scoreMenu;
    private ScoreMenu _scoreMenu;

    public GameObject pauseMenu;
    private bool isPaused;

    public GameObject nextButton;
    public GameObject prvsButton;

    public GameObject HUD;

    public GameObject startMenu;

    public GameObject tutorialMenu;


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
        HUD.SetActive(false);
        scoreMenu.SetActive(false);
        startMenu.SetActive(true);
        tutorialMenu.SetActive(false);
    }

    void Update()
    {
        //Enable next button if not on the last level
        if (_currentLevel == _levels.Length - 1)
            nextButton.SetActive(false);
        else
            nextButton.SetActive(true);

        //Enable previouse button if not on the first level
        if (_currentLevel == 0)
        {
            prvsButton.SetActive(false);
        }
        else
            prvsButton.SetActive(true);

        //Enable pause menu apprpriatly. 
        if (isPaused == true && !scoreMenu.activeSelf && !startMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        //Toggle pause on esc key press 
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
          
    }

    public void StartLevel()
    {
        HUD.SetActive(true);
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

        GameDataModel.GlideValue = 50;

        _scoreMenu.ClearScore();
    }

    private void AddToDictionary()
    {

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

    public void PreviouseLevel() {
                    
            _levels[_currentLevel].SetActive(false);
            GameDataModel.DesObjDictionary.Clear();
            _currentLevel--;

            for (int i = 0; i < _resetables.Count; i++)
            {
                _resetables[i].Revert();
            }

            _levels[_currentLevel].SetActive(true);
            StartLevel();

    }


    public void Quit() {
        Application.Quit();
    }

    public void TogglePause() {
        if (isPaused == true)
            isPaused = false;
        else
            isPaused = true;
    }

    public void OpenTutorial() {
        tutorialMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void CloseTutorial() {
        tutorialMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    





}
