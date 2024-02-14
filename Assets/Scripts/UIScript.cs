using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}


    public void SetTimer(float timer)
    {
        _timerText.text = TimeSpan.FromSeconds(timer).ToString("mm:ss");
    }

    public void SetTimer(int score)
    {
        _scoreText.text = score.ToString();
    }
}
