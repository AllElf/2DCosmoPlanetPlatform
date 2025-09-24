using System.Collections;
using UnityEngine;

public class SearchDistance : MonoBehaviour
{
    [SerializeField] CameraShake _cameraShake;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] GameObject _lootObject;
    [SerializeField] float _lootChance = 0.5f; // 50% шанс выпадения лута
    [SerializeField] float _distance;
    [SerializeField] float _minDistance, _currentRange, _maxRange;
    Vector3 _target;
    [SerializeField] bool isDestroy = false;

    private void Start()
    {
        _cameraShake = GameObject.FindObjectOfType<CameraShake>();
    }

    private void Update()
    {
        _target = new Vector3(transform.position.x, _minDistance, transform.position.z);
        DistanceSearch();
    }

    void DistanceSearch()
    {
        _distance = Vector3.Distance(transform.position, _target);
        _currentRange = Random.Range(0, _maxRange);

        if (_distance <= _currentRange && !isDestroy)
        {
            if (_cameraShake != null && _particleSystem != null)
            {
                _cameraShake._shake = true;
                _particleSystem.Play();
                isDestroy = true;
                StartCoroutine(Destroy());
            }
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);

        if (Random.value <= _lootChance) // Проверка выпадения лута
        {
            Transform parentTransform = transform.parent != null ? transform.parent : null;
            GameObject lootInstance = Instantiate(_lootObject, transform.position, Quaternion.identity);

            if (parentTransform != null)
            {
                lootInstance.transform.SetParent(parentTransform); // Устанавливаем родителя
            }
        }

        Destroy(gameObject);
    }
}