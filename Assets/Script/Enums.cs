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
        Ready,  //�߻� �غ�
        Delay   //�߻� �ĵ�
    }

}
