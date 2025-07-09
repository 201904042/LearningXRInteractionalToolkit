using UnityEngine;
using static Enums;
using UnityEngine.XR.Interaction.Toolkit;

public class GunMagazineManager : MonoBehaviour
{
    public Transform attachPoint;
    public Magazine CurrentMag { get; private set; }

    public void Insert(Magazine mag)
    {
        CurrentMag = mag;
        mag.transform.SetParent(attachPoint);
    }

    public void Eject()
    {
        if (CurrentMag == null) return;
        CurrentMag.transform.SetParent(null);
        CurrentMag = null;
    }

 

    public bool MagInserted() => CurrentMag != null;
    public bool HasAmmo() => MagInserted() && CurrentMag.BulletCount > 0;

    public void UseAmmo() => CurrentMag?.UseBullet();
}
