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
        // 첫 번째 손으로 잡은 경우
        if (!isSelected)
        {
            base.OnSelectEntered(args);
        }
        else
        {
            // 두 번째 손으로 잡음
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
            // 부 손 위치를 서브 어태치 포인트 위치에 강제 고정
            var interactorTransform = secondaryInteractor.GetAttachTransform(this);
            interactorTransform.position = secondaryAttachTransform.position;
            interactorTransform.rotation = secondaryAttachTransform.rotation;
        }
    }
}
