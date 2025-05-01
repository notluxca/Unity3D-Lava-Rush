using UnityEngine;

public class PointUpBillboard : MonoBehaviour
{
    public float lifetime = 2f;
    public float moveUpDistance = 2f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float timer = 0f;
    private Vector3 startPosition;
    private MeshRenderer meshRenderer;
    private Camera mainCamera;
    private Material material;

    void Start()
    {
        startPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        mainCamera = Camera.main;

        // Duplicate the material so it doesn't affect others using the same one
        material = meshRenderer.material;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Always face the camera
        transform.forward = mainCamera.transform.forward;

        // Update the timer
        timer += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(timer / lifetime);

        // Move upwards
        float height = moveCurve.Evaluate(normalizedTime) * moveUpDistance;
        transform.position = startPosition + Vector3.up * height;

        // Fade out
        if (material.HasProperty("Color"))
        {
            Color color = material.color;
            color.a = 1f - normalizedTime;
            material.color = color;
        }

        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
