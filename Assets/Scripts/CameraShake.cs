using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera targetCamera; // Конкретная камера
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    [SerializeField] public bool _shake;

    private Vector3 originalPosition;

    private void Start()
    {
        _shake = false;
        if (targetCamera == null)
            targetCamera = Camera.main; // Если не назначено, берём основную камеру

        originalPosition = targetCamera.transform.localPosition;
    }
    private void Update()
    {
        if(_shake)
        {
            TriggerShake();
            _shake = false;
        }
    }
    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            targetCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;

            yield return null;
        }

        targetCamera.transform.localPosition = originalPosition;
    }
}