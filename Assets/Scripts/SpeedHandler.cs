using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedHandler : MonoBehaviour
{
    [SerializeField] Rigidbody playerRigidbody;
    AudioManager audioManager;

    float currentWindVolume = 0;
    [SerializeField] float minSpeed = 0;
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float lerpSpeed = .1f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("WindSFX");
    }

    // Update is called once per frame
    void Update()
    {
        float playerVelocity = (playerRigidbody.velocity.magnitude);
        currentWindVolume = Mathf.Lerp(currentWindVolume, Mathf.InverseLerp(minSpeed, maxSpeed, playerVelocity), lerpSpeed);
        ChangeVolume("WindSFX", currentWindVolume);
    }
    public void ChangeVolume(string name, float newVolume)
    {
        Sound s = Array.Find(audioManager.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = newVolume;
    }
}
