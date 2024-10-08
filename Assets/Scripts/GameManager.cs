using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Utilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public UIScript UI;
    private float _timer;
    public bool isRacing { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRacing)
        {
            _timer += Time.deltaTime;
            UI.SetTimer(_timer);
        }

    }

    public void ToggleRacing(bool state)
    {
        isRacing = state;
        _timer = 0;
        UI.SetTimer(_timer);
        UI.ToggleTimer(state);
        RaceStarter.ToggleVisibilityAll(!state);
    }
}
