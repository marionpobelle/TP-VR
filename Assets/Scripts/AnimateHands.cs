using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{

    private Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = GetComponent<Animator>();
    }

}
