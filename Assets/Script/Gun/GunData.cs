using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    [Header("스텟")]
    [SerializeField] private int gunId;            //아이디
    [SerializeField] private string gunName;          //이름
    [SerializeField] private string gunDescription;   //설명
    [SerializeField] private float gunDamage;            //데미지
    [SerializeField] private float gunFireRate;          //발사속도
    [SerializeField] private int gunBulletCount;         //장전개수
    [SerializeField] private Enums.GunType gunType;      //총의 타입(임시) 필요한가?

    [Header("오브젝트")]
    public GameObject bulletPrefab; //총알 프리팹
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
