﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour {

    public Text gradingText;
    public Text score;


    public int _bronzeLevel;
    public int _silverLevel;
    public int _goldLevel;

    private string _grading;
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
            gradingText.text = "Ungradable :(!";
        else if (_points < _silverLevel)
            gradingText.text = "bronze!";
        else if (_points < _goldLevel)
            gradingText.text = "Silver!";
        else
            gradingText.text = "Gold!";

    }

}

