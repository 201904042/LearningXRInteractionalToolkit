using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShootLine_Controller : MonoBehaviour
{
    [Header("XRI Default Input Actions")]
    public InputActionAsset inputActions;

    [Header("조준선 설정")]
    public float maxDistance = 50f;
    public LayerMask hitLayers;

    [Header("색상")]
    public Color normalColor = Color.green;
    public Color hitColor = Color.red;

    private LineRenderer lineRenderer;
    private InputAction toggleAimAction;
    private bool isActive = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 액션 찾기
        toggleAimAction = inputActions.FindAction("XRI Left/ToggleAimLine");
        if (toggleAimAction != null)
        {
            toggleAimAction.performed += OnYButtonPressed;
            toggleAimAction.Enable();
        }
        else
        {
            Debug.LogError("ToggleAimLine 액션을 찾을 수 없습니다.");
        }
    }

    private void OnDisable()
    {
        if (toggleAimAction != null)
        {
            toggleAimAction.performed -= OnYButtonPressed;
            toggleAimAction.Disable();
        }
    }

    private void OnYButtonPressed(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        lineRenderer.enabled = isActive;
    }

    void Update()
    {
        if (!isActive || transform == null) return;

        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = start + direction * maxDistance;

        if (Physics.Raycast(start, direction, out RaycastHit hit, maxDistance, hitLayers))
        {
            end = hit.point;
            lineRenderer.startColor = hitColor;
            lineRenderer.endColor = hitColor;
        }
        else
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

}
