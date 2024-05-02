using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimation;
    public InputActionProperty gripAnimation;
    public Animator handAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Grip", gripValue);
    }
}
