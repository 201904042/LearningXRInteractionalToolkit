using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SlideHandle : MonoBehaviour
{
    public Transform slideStart;
    public Transform slideEnd;
    [Range(0f, 1f)] public float threshold = 0.8f;

    private XRGrabInteractable grab;
    private Vector3 initialLocalPos;
    private bool isPulled;
    private GunObject gun;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        initialLocalPos = transform.localPosition;
        gun = GetComponentInParent<GunObject>();
    }

    private void Update()
    {
        // ���� ��� ���� ���� �����̵� �׷� Ȱ��ȭ
        grab.enabled = gun.grabInteractable.isSelected;

        if (ShouldForceRelease())
        {
            ForceRelease();
            return;
        }

        if (IsGrabActive())
        {
            UpdateSlidePosition();
            CheckAndTriggerPull();
        }
        else
        {
            ResetSlide();
        }
    }

    private bool ShouldForceRelease()
    {
        //���� ������� �ʰ� �����̵带 ���� ���
        return !gun.grabInteractable.isSelected && grab.isSelected;
    }

    //���� ����
    private void ForceRelease()
    {
        isPulled = false;
        transform.localPosition = initialLocalPos;

        if (grab.interactorsSelecting.Count > 0)
        {
            grab.interactionManager.SelectExit(grab.interactorsSelecting[0], grab);
        }
    }

    private bool IsGrabActive()
    {
        //���� ���� ���� ���¿��� �����̵带 ����ִ°�
        return gun.grabInteractable.isSelected && grab.isSelected;
    }

    private void UpdateSlidePosition()
    {
        Vector3 direction = slideEnd.position - slideStart.position;
        float maxDistance = direction.magnitude;

        Vector3 projected = Vector3.Project(transform.position - slideStart.position, direction.normalized);
        float pulledRatio = projected.magnitude / maxDistance;

        transform.position = slideStart.position + Vector3.ClampMagnitude(projected, maxDistance);

        if (!isPulled && pulledRatio >= threshold)
        {
            isPulled = true;
        }
    }

    private void CheckAndTriggerPull()
    {
        if (isPulled)
        {
            gun.PullSlider();
            // �ѹ� ���� �� �ٽ� üũ���� �ʵ��� ���� �ʱ�ȭ �� �ʿ��ϸ� ���⼭ ó��
            isPulled = false;
        }
    }

    private void ResetSlide()
    {
        isPulled = false;
        transform.localPosition = initialLocalPos;
    }
}
