using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class XRControllerBtnInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset actionAsset;

    [Header("Unity Events")]
    public UnityEvent onLeftPrimaryPressed;    // X 버튼
    public UnityEvent onLeftSecondaryPressed;  // Y 버튼
    public UnityEvent onRightPrimaryPressed;   // A 버튼
    public UnityEvent onRightSecondaryPressed; // B 버튼

    private Dictionary<string, (InputAction action, UnityEvent unityEvent)> actionEventMap;

    private void Awake()
    {
        actionEventMap = new Dictionary<string, (InputAction, UnityEvent)>
        {
            { "XRI Left/PressPrimaryButton",    (null, onLeftPrimaryPressed) },
            { "XRI Left/PressSecondaryButton",  (null, onLeftSecondaryPressed) },
            { "XRI Righ/PressPrimaryButton",   (null, onRightPrimaryPressed) },
            { "XRI Right/PressSecondaryButton", (null, onRightSecondaryPressed) }
        };
    }

    private void OnEnable()
    {
        if (actionAsset == null)
        {
            Debug.LogWarning("InputActionAsset이 설정되지 않았습니다.");
            return;
        }

        // 임시 저장용 리스트
        List<KeyValuePair<string, (InputAction, UnityEvent)>> updatedEntries = new();

        foreach (var kvp in actionEventMap)
        {
            var key = kvp.Key;
            var unityEvent = kvp.Value.unityEvent;

            var action = actionAsset.FindAction(key);
            if (action != null)
            {
                action.performed += ctx => unityEvent?.Invoke();
                action.Enable();
                updatedEntries.Add(new KeyValuePair<string, (InputAction, UnityEvent)>(key, (action, unityEvent)));
            }
            else
            {
                Debug.LogWarning($"{key} 액션을 찾을 수 없습니다.");
            }
        }

        // 루프 후 Dictionary 업데이트
        foreach (var entry in updatedEntries)
        {
            actionEventMap[entry.Key] = entry.Value;
        }

        Debug.Log("Custom Button Action Complete");
    }


    private void OnDisable()
    {
        foreach (var entry in actionEventMap.Values)
        {
            entry.action?.Disable();
        }
    }

    // Debug 용 메서드 (인스펙터에서 연결 가능)
    public void DebugXButton() => Debug.Log("X 버튼 눌림");
    public void DebugYButton() => Debug.Log("Y 버튼 눌림");
    public void DebugAButton() => Debug.Log("A 버튼 눌림");
    public void DebugBButton() => Debug.Log("B 버튼 눌림");
}
