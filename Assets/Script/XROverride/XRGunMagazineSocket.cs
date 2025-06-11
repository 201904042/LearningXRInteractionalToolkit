using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGunMagazineSocket : XRSocketInteractor
{
    private GunObject gun; // ������ �� ��ü

    protected override void Awake()
    {
        base.Awake();
        gun = GetComponentInParent<GunObject>();
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // �⺻ ������ �´��� üũ
        if (!base.CanSelect(interactable))
            return false;

        // �̹� �ѿ� źâ�� ������ ������ ���� �Ұ�
        if (gun != null && gun.HasMagazineAttached())
            return false;

        return true;
    }
}
