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
        UnGrab,  //���� ���� ����
        NoMag,   //źâ�� ����
        NoSlide, //�����̵带 ����� ����
        NoAmmo,  //źâ�� ������ źȯ�� ����
        Ready,   //�߻� �غ�
        Delay    //�߻� �ĵ�
    }

}
