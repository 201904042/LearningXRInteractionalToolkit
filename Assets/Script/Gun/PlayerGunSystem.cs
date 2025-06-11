using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Enums;


public class PlayerGunSystem : MonoBehaviour
{
    [Header("InputAction")]
    public InputActionAsset inputActions;
    private InputAction FirstButton;
    private InputAction GripButton;
    private InputAction TriggerButton;

    [SerializeField] private GunObject leftGun;
    [SerializeField] private GunObject rightGun;

    public void Awake()
    {
        ActivateInputSystem();
    }

    private void ActivateInputSystem()
    {
        FirstButton = inputActions.FindAction("XRI Left/PressPrimaryButton");
        if (FirstButton == null)
        {
            Debug.LogError("FirstButton 액션을 찾을 수 없습니다.");
        }

        TriggerButton = inputActions.FindAction("XRI Left/PressTriggerButton");
        if (FirstButton == null)
        {
            Debug.LogError("TriggerButton 액션을 찾을 수 없습니다.");
        }

        GripButton = inputActions.FindAction("XRI Left/PressGripButton");
        if (GripButton == null)
        {
            Debug.LogError("GripButton 액션을 찾을 수 없습니다.");
        }


        FirstButton.performed += OnXButtonPressed;
        FirstButton.Enable();
        TriggerButton.performed += OnTriggerPressed;
        TriggerButton.Enable();
        GripButton.performed += OnGripPressed;
        GripButton.Enable();
    }

    private void OnGripPressed(InputAction.CallbackContext context)
    {
        
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        
    }

    private void OnXButtonPressed(InputAction.CallbackContext context)
    {
        
    }

    
}
