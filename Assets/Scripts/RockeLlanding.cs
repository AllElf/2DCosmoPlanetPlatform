using UnityEngine;
using System.Collections;

public class RocketLanding : MonoBehaviour
{
    [SerializeField] private Hints m_Hints;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _cameraRocket;
    [SerializeField] private GameObject _rocket;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _landingRocket;
    [SerializeField] private Transform _playerStartPoint;
    [SerializeField] private ParallaxBackground2 _background;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isLanding, _flyingAway,_complete;
    [SerializeField] public float _horizontal;

    private void Start()
    {
        _isLanding = false;
        _flyingAway = false;
        _complete = false;

        if (_player != null)
        {
            _player.SetActive(false);
        }
        if (_background != null)
        {
            _background.enabled = false;
        }
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        Landing();
        FlyingAway();
    }

    void Landing()
    {
        if (_cameraRocket != null && _landingRocket != null && _complete == false)
        {
            if (_cameraRocket.transform.position == _landingRocket.transform.position && _isLanding == false)
            {
                Debug.Log("Rocket Landing");
                _isLanding = true;
            }
        }
    }

    void FlyingAway()
    {
        if (_isLanding && _horizontal != 0)
        {
            if (_player != null)
            {
                _player.SetActive(true);
                _flyingAway = true;
            }
        }

        if (_flyingAway && _horizontal > 0 && _player.transform.position.x >= _playerStartPoint.transform.position.x)
        {
            m_Hints._corutinStop = true;
            _animator.Play("RocketFliesAway");
            StartCoroutine(CheckAnimationCompletion()); // Запускаем корутину
            _complete = true;
            _flyingAway = false;
            _isLanding = false;
            if(m_Hints != null)
            {
                m_Hints._request = true;
            }
        }
    }

    IEnumerator CheckAnimationCompletion()
    {
        yield return null; // Ждем один кадр, чтобы Animator успел обновиться

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || _animator.IsInTransition(0))
        {
            yield return null; // Ждем, пока анимация завершится
        }
        _cameraRocket.SetActive(false);
        if(_mainCamera != null)
        {
            if(_rocket  != null)
            {
                _rocket.transform.parent = null;
                _rocket.SetActive(false);
            }
            Destroy(_cameraRocket);
            _mainCamera.SetActive(true);
            //Destroy(GetComponent<RocketLanding>()); // Удалить после передачи данных
        }
        Debug.Log("Animation is complete");
    }
}