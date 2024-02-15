using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStarter : MonoBehaviour
{
    [SerializeField]
    static private List<RaceStarter> _raceStarters = new List<RaceStarter>();

    public float Timer;
    private float _currentTimer;
    private bool _isWaiting;

    [SerializeField] private TimeAttackHandler _timeAttackHandler;

    private void Awake()
    {
        _raceStarters.Add(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isWaiting && !GameManager.Instance.isRacing)
        {
            _currentTimer += Time.deltaTime;
            if(_currentTimer >= Timer)
            {
                _isWaiting = false;
                _timeAttackHandler.StartRace();
            }
        }
    }

    public static void ToggleVisibilityAll(bool state)
    {
        foreach (var raceStarter in _raceStarters)
        {
            raceStarter.ToggleVisibility(state);
            raceStarter.ResetWaiting();
        }
    }

    private void ResetWaiting()
    {
        _isWaiting = false;
        _currentTimer = 0;
    }

    public void ToggleVisibility(bool state)
    {
        ToggleWaiting(state);
        gameObject.SetActive(state);
    }

    public void ToggleWaiting(bool state)
    {
        _isWaiting = state;
        _currentTimer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleWaiting(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleWaiting(false);
        }
    }
}
