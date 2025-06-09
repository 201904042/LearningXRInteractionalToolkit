using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum GunType
    {
        Pistol,
        Rifle,
        Shotgun
    }

    public enum GunState
    {
        Ready,  //발사 준비
        NoAmmo, //총알 없음
        NoMag,  //탄알집 없음
        NoLoaded, //탄알집이 결합되었으나 장전되지 않음
        Delay   //발사 후딜
    }

}
