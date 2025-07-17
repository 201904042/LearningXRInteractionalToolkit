using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(LineRenderer))]
public class ShootLine_Controller : MonoBehaviour
{
    [Header("XRI Default Input Actions")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("조준선 설정")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask hitLayers;

    [Header("색상")]
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color hitColor = Color.red;

    private LineRenderer lineRenderer;
    private InputAction toggleAimAction;
    private bool isActive = false;

    void Awake()
    {
        InitializeLineRenderer();
    }

    private void OnEnable()
    {
        SetupInputAction();
    }

    private void OnDisable()
    {
        if (toggleAimAction != null)
        {
            toggleAimAction.performed -= OnToggleAimPerformed;
            toggleAimAction.Disable();
        }
    }

    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.01f); // optional
    }

    private void SetupInputAction()
    {
        if (inputActions == null)
        {
            Debug.LogError("InputActionAsset가 설정되지 않았습니다.");
            return;
        }

        toggleAimAction = inputActions.FindAction("XRI Left/PressSecondaryButton");

        if (toggleAimAction != null)
        {
            toggleAimAction.performed += OnToggleAimPerformed;
            toggleAimAction.Enable();
        }
        else
        {
            Debug.LogError("ToggleAimLine 액션을 찾을 수 없습니다.");
        }
    }

    private void OnToggleAimPerformed(InputAction.CallbackContext context)
    {
        SetLineActive(!isActive);
    }

    public void SetLineActive(bool active)
    {
        isActive = active;
        lineRenderer.enabled = active;
    }

    private void Update()
    {
        if (!isActive || transform == null)
            return;

        UpdateLine();
    }

    private void UpdateLine()
    {
        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = start + direction * maxDistance;

        if (Physics.Raycast(start, direction, out RaycastHit hit, maxDistance, hitLayers))
        {
            end = hit.point;
            SetLineColor(hitColor);
        }
        else
        {
            SetLineColor(normalColor);
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void SetLineColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
