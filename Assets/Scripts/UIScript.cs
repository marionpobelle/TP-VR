using System;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject _go;
    [SerializeField] private TextMeshProUGUI _timerText;

    public void SetTimer(float timer)
    {
        _timerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }

    public void ToggleTimer(bool state) 
    {
        _go.SetActive(state);
    }

}
