using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [Tooltip("이 Zone의 점수")]
    public int scoreValue = 0;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("총알 적중!");
        }
    }
}
