using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const float bulletForce = 1000f;

    [Header("총알 속성")]
    public GameObject modelObject; // 총알 모델 (Mesh)
    public GameObject trailObject; // TrailRenderer 포함 오브젝트
    private Rigidbody rb;
    private TrailRenderer trail;

    public float damage;
    public float maxLifeTime = 3f; // 비충돌 시 자동 제거 시간
    private bool hasHit = false;
    private float destroyDelay = 0.5f; // TrailRenderer 시간보다 약간 여유 있게

    [Header("Trail 설정")]
    public float trailTime = 0.4f;
    public float startWidth = 0.05f;
    public float endWidth = 0f;
    public float minVertexDistance = 0.05f;
    public Gradient trailColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = trailObject.GetComponent<TrailRenderer>();
        SetupTrailRenderer();
    }

    private void SetupTrailRenderer()
    {
        if (trail == null) return;

        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        trail.minVertexDistance = minVertexDistance;

        if (trailColor != null)
            trail.colorGradient = trailColor;

        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.receiveShadows = false;
    }

    public void Init(float dmg, Vector3 direction)
    {
        damage = dmg;
        hasHit = false;

        modelObject.SetActive(true);
        trailObject.SetActive(true);
        trail.Clear();
        SetupTrailRenderer(); // 재사용 시 매번 초기화

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction * bulletForce, ForceMode.Impulse);

        CancelInvoke();
        Invoke(nameof(ForceDestroy), maxLifeTime);
    }

    /// <summary>
    /// 파괴판정
    /// </summary>
    private void ForceDestroy()
    {
        if (!hasHit)
        {
            hasHit = true;
            modelObject.SetActive(false);
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            Invoke(nameof(DestroyBullet), trail.time + destroyDelay);
        }
    }

    /// <summary>
    /// 총알 오브젝트 제거
    /// </summary>
    private void DestroyBullet()
    {
        Destroy(gameObject); // 풀링 시엔 SetActive(false) 대체 가능
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        modelObject.SetActive(false);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        Invoke(nameof(DestroyBullet), trail.time + destroyDelay);
    }
}
