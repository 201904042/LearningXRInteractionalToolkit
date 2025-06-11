using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGunMagazineSocket : XRSocketInteractor
{
    private GunObject gun; // 참조할 총 객체

    protected override void Awake()
    {
        base.Awake();
        gun = GetComponentInParent<GunObject>();
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // 기본 조건이 맞는지 체크
        if (!base.CanSelect(interactable))
            return false;

        // 이미 총에 탄창이 끼워져 있으면 선택 불가
        if (gun != null && gun.HasMagazineAttached())
            return false;

        return true;
    }
}
