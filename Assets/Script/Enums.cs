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
        Empty,
        UnGrab,
        Ready,  //�߻� �غ�
        Delay   //�߻� �ĵ�
    }

}
