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
        UnGrab,
        Ready,  //발사 준비
        Delay   //발사 후딜
    }

}
