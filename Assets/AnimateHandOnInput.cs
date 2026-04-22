using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;

    public Animator handAnimator;
    
    void Start()
    {
        // Intentionally left empty: this component requires no initialization.
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();
        float grip = triggerValue.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", trigger);
        handAnimator.SetFloat("Grip", grip);
    }
}
