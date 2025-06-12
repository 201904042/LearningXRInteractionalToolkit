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

        GameObject targetObj = (interactable as MonoBehaviour)?.gameObject;
        Magazine targetMag = targetObj?.GetComponent<Magazine>();

        //���� �����ǰų� Interactable��  źâ�� �ƴ�
        if (gun == null || targetMag == null)
        {
            Debug.Log("���̳� źâ�� ����");
            return false;
        }
            

        // �Ѱ� źâ�� ��ġ���� �ʰų� �̹� �ѿ� źâ�� ������ ������ ���� �Ұ�
        if (gun.gunData.GetId() != targetMag.GetId() || gun.HasMagazineAttached())
        {
            Debug.Log("���̵� ����ġ Ȥ�� �̹� ������");
            return false;
        }
            

        return true;
    }
}
