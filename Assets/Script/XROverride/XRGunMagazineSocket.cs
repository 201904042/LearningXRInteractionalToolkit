using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGunMagazineSocket : XRSocketInteractor
{
    private GunObject gun;

    protected override void Awake()
    {
        base.Awake();
        gun = GetComponentInParent<GunObject>();
    }

    private bool IsValidMagazine(IXRInteractable interactable)
    {
        GameObject targetObj = (interactable as MonoBehaviour)?.gameObject;

        // 총을 잡고 있는지 체크
        Magazine mag = targetObj.GetComponent<Magazine>();

        XRGrabInteractable gunGrab = gun.GetComponent<XRGrabInteractable>();
        if (mag == null || gunGrab == null) 
            return false;
        // 탄창 ID와 총 ID가 일치하는지 + 아직 장착되지 않았는지
        if (!mag.IsOnGrab || gun.HasMagazineAttached() || gun.gunData.GetId() != mag.GetId()) 
            return false;

        return true;
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsValidMagazine(interactable);

    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsValidMagazine(interactable);
    }

}
