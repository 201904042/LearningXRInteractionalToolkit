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
    public UnityEvent onLeftPrimaryPressed;    // X ��ư
    public UnityEvent onLeftSecondaryPressed;  // Y ��ư
    public UnityEvent onRightPrimaryPressed;   // A ��ư
    public UnityEvent onRightSecondaryPressed; // B ��ư

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
            Debug.LogWarning("InputActionAsset�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �ӽ� ����� ����Ʈ
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
                Debug.LogWarning($"{key} �׼��� ã�� �� �����ϴ�.");
            }
        }

        // ���� �� Dictionary ������Ʈ
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

    // Debug �� �޼��� (�ν����Ϳ��� ���� ����)
    public void DebugXButton() => Debug.Log("X ��ư ����");
    public void DebugYButton() => Debug.Log("Y ��ư ����");
    public void DebugAButton() => Debug.Log("A ��ư ����");
    public void DebugBButton() => Debug.Log("B ��ư ����");
}
