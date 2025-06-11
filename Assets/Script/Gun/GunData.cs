using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    [Header("����")]
    [SerializeField] private int gunId;            //���̵�
    [SerializeField] private string gunName;          //�̸�
    [SerializeField] private string gunDescription;   //����
    [SerializeField] private float gunDamage;            //������
    [SerializeField] private float gunFireRate;          //�߻�ӵ�
    [SerializeField] private int gunBulletCount;         //��������
    [SerializeField] private Enums.GunType gunType;      //���� Ÿ��(�ӽ�) �ʿ��Ѱ�?

    [Header("������Ʈ")]
    public GameObject bulletPrefab; //�Ѿ� ������
    public GameObject magPrefab;

    public int GetId()
    {
        return gunId;
    }
    public string GetName()
    {
        return gunName;
    }
    public string GetDescription()
    {
        return gunDescription;
    }
    public float GetDamage()
    {
        return gunDamage;
    }
    public float GetFireRate()
    {
        return gunFireRate;
    }
    public int GetBulletCount()
    {
        return gunBulletCount;
    }
    public Enums.GunType GetGunType()
    {
        return gunType;
    }
}
