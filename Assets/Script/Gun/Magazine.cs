using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int id;         //종류별 아이디
    [SerializeField] private int bullets;    //장전된 총알의 수

    public bool autoInit = false;

    public void Awake()
    {
        id = 0;
        bullets = 10;
    }


    public void Init(int id, int num)
    {
        this.id = id;   
        bullets = num;
    }

    public void SetBullets(int num)
    {
        bullets = num;
    }

    public int GetId()
    {
        return id;
    }
    public int GetBullets()
    {
        return bullets;
    }
}
