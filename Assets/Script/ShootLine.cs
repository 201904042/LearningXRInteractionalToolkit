using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootLine : MonoBehaviour
{
    [Header("�ѱ� ��ġ")]
    public Transform firePoint;

    [Header("���ؼ� ����")]
    public float maxDistance = 50f;
    public LayerMask hitLayers;

    [Header("����")]
    public Color normalColor = Color.green;
    public Color hitColor = Color.red;

    private LineRenderer lineRenderer;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (!isHeld || firePoint == null) return;

        Vector3 start = firePoint.position;
        Vector3 direction = firePoint.forward;
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

    //�̺�Ʈ
    public void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        lineRenderer.enabled = true;
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        lineRenderer.enabled = false;
    }

    
}
