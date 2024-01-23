using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripReference;
    [SerializeField] private InputActionReference triggerReference;
    [SerializeField] private Animator animator;
    // Update is called once per frame
    private String Grip = "Grip";
    private String Trigger = "Trigger";
    void Update()
    {
        float gripValue = gripReference.action.ReadValue<float>();
        float triggerValue = triggerReference.action.ReadValue<float>();
        animator.SetFloat(Grip, gripValue);
        animator.SetFloat(Trigger, triggerValue);
    }
}
