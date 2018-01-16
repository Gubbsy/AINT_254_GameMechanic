using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour {

    public Text gradingText;
    public Text score;


    public int _bronzeLevel;
    public int _silverLevel;
    public int _goldLevel;

    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;
    public GameObject rubbish;


    private int _points;



    // Use this for initialization
    void Start() {

        
        Time.timeScale = 0.0f;
    }

    public void UpateScore()
    {
        _points = GameDataModel.Points;
        score.text = _points.ToString();

        if (_points < _bronzeLevel)
            rubbish.SetActive(true);
        else if (_points < _silverLevel)
            bronze.SetActive(true);
        else if (_points < _goldLevel)
            silver.SetActive(true);
        else
            gold.SetActive(true);

        ClearScore();

    }

    public void ClearScore() {
        bronze.SetActive(false);
        silver.SetActive(false);
        gold.SetActive(false);
        rubbish.SetActive(false);
    }

}

