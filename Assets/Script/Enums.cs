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
        UnGrab,  //총을 잡지 않음
        NoMag,   //탄창이 없음
        NoSlide, //슬라이드를 당기지 않음
        NoAmmo,  //탄창은 있으나 탄환이 없음
        Ready,   //발사 준비
        Delay    //발사 후딜
    }

}
