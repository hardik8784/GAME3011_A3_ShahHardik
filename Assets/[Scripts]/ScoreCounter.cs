///----------------------------------------------------------------------------------
///   GAME3011_A3_ShahHardik
///   ScoreCounter.cs
///   Author            : Hardik Dipakbhai Shah
///   Last Modified     : 2022/03/23
///   Description       : 
///   Revision History  : 1st ed.   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;

            _score = value;

            scoreText.SetText("Score : " + _score);
        }
    }

    [SerializeField]
    private TextMeshProUGUI scoreText;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
