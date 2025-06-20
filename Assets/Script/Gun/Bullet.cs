using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Header("총알 속성")]
    public float maxLifeTime = 3f;

    [Header("Trail 설정")]
    public float trailTime = 0.4f;
    public float startWidth = 0.05f;
    public float endWidth = 0f;
    public float minVertexDistance = 0.05f;
    public Gradient trailColor;

    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
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

    public void InitVisual(Vector3 start, Vector3 end)
    {
        trailObject.SetActive(true);
        trail.Clear();
        SetupTrailRenderer();

        transform.position = start;
        StartCoroutine(MoveToTarget(end));
    }

    private IEnumerator MoveToTarget(Vector3 end)
    {
        float duration = trailTime; // 비주얼 이동 시간
        float time = 0f;

        Vector3 start = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        ForceDestroy();
    }

    private void ForceDestroy()
    {
        Invoke(nameof(DestroyBullet), trail.time + 0.5f);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject); // 풀링 시에는 SetActive(false)
    }
}
