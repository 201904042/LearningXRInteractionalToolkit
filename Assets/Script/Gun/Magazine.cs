using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int id;         //������ ���̵�
    [SerializeField] private int bullets;    //������ �Ѿ��� ��

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
