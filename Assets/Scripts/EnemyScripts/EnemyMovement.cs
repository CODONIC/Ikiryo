using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private bool _isChasing;

    [Header("Audio Settings")]
    public AudioSource backgroundMusicSource;
    public AudioSource chaseMusicSource;
    public float fadeDuration = 1f;
    private Coroutine currentCoroutine = null;
    private bool isChaseMusicPlaying = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    private void Start()
    {
        backgroundMusicSource.Play();
        chaseMusicSource.Stop();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        ManageAudio();
    }

    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
            _isChasing = true;
        }
        else
        {
            _targetDirection = Vector2.zero;
            _isChasing = false;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.MoveRotation(rotation);
    }

    private void SetVelocity()
    {
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = transform.up * _speed;
        }
    }

    private void ManageAudio()
    {
        if (_playerAwarenessController.AwareOfPlayer && !isChaseMusicPlaying)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeToChaseMusic());
            isChaseMusicPlaying = true;
        }
        else if (!_playerAwarenessController.AwareOfPlayer && isChaseMusicPlaying)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeToBackgroundMusic());
            isChaseMusicPlaying = false;
        }
    }

    IEnumerator FadeToChaseMusic()
    {
        float timer = 0f;
        chaseMusicSource.volume = 0f;
        chaseMusicSource.Play();

        while (timer < fadeDuration)
        {
            backgroundMusicSource.volume = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            chaseMusicSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        backgroundMusicSource.Pause();
        chaseMusicSource.volume = 1f;
        currentCoroutine = null;
    }

    IEnumerator FadeToBackgroundMusic()
    {
        float timer = 0f;
        backgroundMusicSource.volume = 0f;
        backgroundMusicSource.UnPause();

        while (timer < fadeDuration)
        {
            chaseMusicSource.volume = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            backgroundMusicSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        chaseMusicSource.Stop();
        backgroundMusicSource.volume = 1f;
        currentCoroutine = null;
    }
}
