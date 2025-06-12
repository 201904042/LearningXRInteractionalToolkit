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

        GameObject targetObj = (interactable as MonoBehaviour)?.gameObject;
        Magazine targetMag = targetObj?.GetComponent<Magazine>();

        //총이 누락되거나 Interactable이  탄창이 아님
        if (gun == null || targetMag == null)
        {
            Debug.Log("총이나 탄창이 없음");
            return false;
        }
            

        // 총과 탄창이 일치하지 않거나 이미 총에 탄창이 끼워져 있으면 선택 불가
        if (gun.gunData.GetId() != targetMag.GetId() || gun.HasMagazineAttached())
        {
            Debug.Log("아이디 불일치 혹은 이미 장전됨");
            return false;
        }
            

        return true;
    }
}
