using UnityEngine;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    [Header("Attach Points")]
    public Transform secondaryAttachTransform;

    private IXRSelectInteractor secondaryInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // ù ��° ������ ���� ���
        if (!isSelected)
        {
            base.OnSelectEntered(args);
        }
        else
        {
            // �� ��° ������ ����
            secondaryInteractor = args.interactorObject;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject == secondaryInteractor)
        {
            secondaryInteractor = null;
        }
        else
        {
            base.OnSelectExited(args);
        }
    }

    void Update()
    {
        if (secondaryInteractor != null && secondaryAttachTransform != null)
        {
            // �� �� ��ġ�� ���� ����ġ ����Ʈ ��ġ�� ���� ����
            var interactorTransform = secondaryInteractor.GetAttachTransform(this);
            interactorTransform.position = secondaryAttachTransform.position;
            interactorTransform.rotation = secondaryAttachTransform.rotation;
        }
    }
}
