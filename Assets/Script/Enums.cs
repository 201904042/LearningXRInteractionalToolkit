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
        Ready,  //�߻� �غ�
        NoAmmo, //�Ѿ� ����
        NoMag,  //ź���� ����
        NoLoaded, //ź������ ���յǾ����� �������� ����
        Delay   //�߻� �ĵ�
    }

}
