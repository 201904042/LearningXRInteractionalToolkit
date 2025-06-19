using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGunMagazineSocket : XRSocketInteractor
{
    [SerializeField] private GunObject m_gun;

    protected override void Awake()
    {
        base.Awake();
        m_gun = m_gun ?? GetComponentInParent<GunObject>();
    }

    private Magazine GetValidMagazine(IXRInteractable interactable)
    {
        if (interactable is not MonoBehaviour mb) 
            return null;
        var magazine = mb.GetComponent<Magazine>();
        var gunGrab = m_gun?.GetComponent<XRGrabInteractable>();

        if (magazine == null || gunGrab == null)
            return null;

        bool isValid = m_gun.gunData.GetId() == magazine.GunId;

        return isValid ? magazine : null;
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && GetValidMagazine(interactable) != null;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && GetValidMagazine(interactable) != null;
    }
}
